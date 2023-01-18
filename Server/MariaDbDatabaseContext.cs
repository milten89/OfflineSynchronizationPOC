using Microsoft.EntityFrameworkCore;
using OfflineSynchronizationPOC.EntityModel;

namespace OfflineSynchronizationPOC.Server
{
    public class MariaDbDatabaseContext : DatabaseContext
    {
        public const string ConnectionString = "server=localhost;port=40000;database=testDb;user=root;password=rootpass";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(ConnectionString, 
                                    new MariaDbServerVersion(new Version(10, 9, 4)));
        }
    }
}
