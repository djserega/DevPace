using Microsoft.EntityFrameworkCore;

namespace DevPace.DbConnectionSQLite
{
    public class DatabaseConnector : DbContext, IDatabaseConnector
    {
        public DatabaseConnector()
        {
            Database.EnsureCreated();

            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            //=> options.UseSqlite("Data Source=:memory:");
            => options.UseSqlite("Data Source=database.db");

        public DbSet<Models.Customer> Customers { get; set; }
    }
}