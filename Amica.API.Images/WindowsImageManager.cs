using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO.Compression;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Drawing.Image;

namespace Amica.API.Images {
    public class WindowsImageManager : IImageManager {
        public string RootStoragePath { get; set; }

        public WindowsImageManager() {
            RootStoragePath = "ImagesStorage";
        }

        private async Task InitializeAsync() {
            var folders = Enum.GetNames(typeof(ImageScale));
            await Task.Run(() => {
                foreach ( var folder in folders ) {
                    Directory.CreateDirectory(Path.Combine(RootStoragePath, folder));
                }
            });
        }

        #region Create
        public async Task<ImageData?> CreateImageAsync(byte[] image) {
            await InitializeAsync();
            Guid fileId = Guid.NewGuid();
            ImageFormat format;
            try {
                /*
                 
                 */


                // write original without resizing and compression
                format = await CreateScaledImageAsync(image, ImageScale.Original, fileId.ToString(), 80L);

                // write half-size
                await CreateScaledImageAsync(image, ImageScale.Half, fileId.ToString(), 30L);

                // write quarter-size
                await CreateScaledImageAsync(image, ImageScale.Quarter, fileId.ToString(), 30L);
            }
            catch {
                return null;
            }
            return new ImageData() {
                ID = fileId,
                Format = format
            };
        }
        private async Task<ImageFormat> CreateScaledImageAsync(byte[] imageArr, ImageScale scale, string destination, long compression) {
            return await Task.Run(async () => {
                using var image = Image.FromStream(new MemoryStream(imageArr));

                //// no need to scale processing
                //if ( scale == ImageScale.Original ) {
                //    await File.WriteAllBytesAsync(GetDest(destination, scale, image.RawFormat), imageArr);
                //    return;
                //}

                int newW = image.Width / (int)scale;
                int newH = image.Height / (int)scale;

                using var dest = new Bitmap(newW, newH);
                var destRect = new Rectangle(0, 0, newW, newH);

                dest.SetResolution(image.HorizontalResolution / 2F, image.VerticalResolution / 2F);

                using var g = Graphics.FromImage(dest);
                g.CompositingMode = CompositingMode.SourceCopy;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using var wrapMode = new ImageAttributes();
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);

                g.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);

                var encoder = await GetEncoderAsync(image.RawFormat);
                System.Drawing.Imaging.Encoder eq = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                EncoderParameter myEncoderParameter = new EncoderParameter(eq, compression);
                myEncoderParameters.Param[0] = myEncoderParameter;

                dest.Save(GetDest(destination, scale, image.RawFormat), encoder, myEncoderParameters);
                return image.RawFormat;
            });
        }
        private async Task<ImageCodecInfo?> GetEncoderAsync(ImageFormat format) {
            return await Task.Run(() => {
                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                foreach ( ImageCodecInfo codec in codecs ) {
                    if ( codec.FormatID == format.Guid ) {
                        return codec;
                    }
                }
                return null;
            });
        }
        #endregion

        #region Delete
        public async Task DeleteFileAsync(Guid id) {
            throw new NotImplementedException();
        }
        #endregion

        #region Get
        public async Task<ImageData?> GetImageAsync(string filename, ImageScale scale) {
            return new ImageData() {
                Data = await File.ReadAllBytesAsync(GetDest(filename, scale))
            };
        }
        #endregion



        private string GetDest(string filename, ImageScale scale, ImageFormat? format = null) {
            if ( format is not null )
                return Path.Combine(
                                Path.Combine(RootStoragePath, Enum.GetName(scale) ?? throw new Exception("error300")),
                                $"{filename}.{format.ToString().ToLower()}");
            else
                return Path.Combine(
                                Path.Combine(RootStoragePath, Enum.GetName(scale) ?? throw new Exception("error300")),
                                $"{filename}");
        }
    }
}
