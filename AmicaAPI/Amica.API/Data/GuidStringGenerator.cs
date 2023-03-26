using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Amica.API.Data {
    internal class GuidStringGenerator : ValueGenerator<string> {
        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry) {
            return Guid.NewGuid().ToString();
        }
    }
}
