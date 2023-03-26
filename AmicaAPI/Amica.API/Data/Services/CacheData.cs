namespace Amica.API.WebServer.Data.Services {
    class CacheData {
        public int? Value { get; set; }
        public DateTimeOffset Expiration { get; set; }
        public override string ToString() {
            return $"Value: {Value}. Will expired at: {Expiration}";
        }
    }
}
