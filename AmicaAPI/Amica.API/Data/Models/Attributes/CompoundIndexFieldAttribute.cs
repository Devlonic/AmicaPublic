namespace Amica.API.Data.Models.Attributes {
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    sealed class CompoundIndexFieldAttribute : Attribute {
        public CompoundIndexFieldAttribute() {
        }
    }
}
