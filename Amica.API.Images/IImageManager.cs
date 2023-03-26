namespace Amica.API.Images {

    public interface IImageManager {
        public string RootStoragePath { get; set; }
        Task<ImageData?> CreateImageAsync(byte[] image);
        Task<ImageData?> GetImageAsync(string filename, ImageScale scale);
        Task DeleteFileAsync(Guid id);
    }
}
