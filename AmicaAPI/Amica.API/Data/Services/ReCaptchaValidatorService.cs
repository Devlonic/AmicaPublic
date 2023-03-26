using Amica.API.WebServer.ConfigurationManagers;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Amica.API.WebServer.Data.Services {
    public class ReCaptchaResponce {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("error-codes")]
        public IEnumerable<string>? ErrorCodes { get; set; }

        [JsonPropertyName("score")]
        public double Score { get; set; }
    }
    public class ReCaptchaValidatorService : ICaptchaValidatorService {
        private readonly IHttpClientFactory httpFactory;
        private readonly ReCaptchaOptions options;

        public ReCaptchaValidatorService(IHttpClientFactory httpFactory, IOptions<ReCaptchaOptions> options) {
            this.httpFactory = httpFactory;
            this.options = options.Value;
        }
        public async Task<bool> IsCaptchaPassedAsync(string token) {
            var responce = await GetCaptchaResultAsync(token);
            return responce?.Success??false;
        }
        private async Task<ReCaptchaResponce?> GetCaptchaResultAsync(string token) {
            //var content = new FormUrlEncodedContent(new[] {
            //    new KeyValuePair<string, string>("secret", options.PrivateKeyV2),
            //    new KeyValuePair<string, string>("responce", token),
            //});
            using var http = httpFactory.CreateClient();
            var uri = string.Format(options.RemoteAddress, options.PrivateKeyV2, token);
            var responce = await http.GetAsync(uri);
            if ( responce.StatusCode != HttpStatusCode.OK )
                throw new HttpRequestException(responce.ReasonPhrase);
            var result = await responce.Content.ReadAsStringAsync();
            var responceObj = JsonSerializer.Deserialize(result, typeof(ReCaptchaResponce)) as ReCaptchaResponce;
            return responceObj;
        }
    }
}
