namespace Amica.API.WebServer.ConfigurationManagers {
    public class ReCaptchaOptions {
        public string RemoteAddress { get; set; } = String.Empty;
        public double AcceptableScore { get; set; }

        public string PublicKeyV2 { get; set; } = String.Empty;
        public string PrivateKeyV2 { get; set; } = String.Empty;
    }
}
