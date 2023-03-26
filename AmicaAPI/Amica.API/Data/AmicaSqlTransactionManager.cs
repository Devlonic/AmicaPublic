using Amica.API.Data;

namespace Amica.API.WebServer.Data
{
    public class AmicaSqlTransactionManager
    {
        private readonly AmicaDbContext db;

        public AmicaSqlTransactionManager(AmicaDbContext db)
        {
            this.db = db;
        }

        public async Task Begin()
        {
            await db.Database.BeginTransactionAsync();
        }
        public async Task Rollback()
        {
            await db.Database.RollbackTransactionAsync();
        }
        public async Task Commit()
        {
            await db.Database.CommitTransactionAsync();
        }
    }
}