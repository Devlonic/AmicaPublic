using System.Drawing.Imaging;
using System.Net.Mime;

namespace Amica.API.Images {
    public class ImageData {
        public Guid? ID { get; set; }
        public ImageFormat? Format { get; set; }
        public byte[]? Data { get; set; }
        public string? ContentType { get; set; }
    }
}
