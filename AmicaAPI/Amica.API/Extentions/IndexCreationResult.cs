namespace Amica.API.WebServer.Extentions {
    public class IndexCreationResult {
        public bool Success { get; init; }
        public bool Created { get; init; }
        public Exception? Exception { get; init; }
    }
}
