using Microsoft.EntityFrameworkCore;
using OfflineSynchronizationPOC.EntityModel;

namespace OfflineSynchronizationPOC.Client
{
    internal class SqliteDatabaseContext : DatabaseContext
    {
        public const string ConnectionString = "Data Source=sqlite.db;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString);
        }
    }
}
