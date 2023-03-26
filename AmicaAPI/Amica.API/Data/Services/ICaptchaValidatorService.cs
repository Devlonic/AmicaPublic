namespace Amica.API.WebServer.Data.Services {
    public interface ICaptchaValidatorService {
        Task<bool> IsCaptchaPassedAsync(string token);
    }
}
