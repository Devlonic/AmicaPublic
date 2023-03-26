namespace Amica.API.WebServer.Controllers.Attributes {
    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    sealed class ThrottleAttribute : Attribute {
        public ThrottleAttribute(long limit) {

        }
    }
}