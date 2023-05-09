using Microsoft.EntityFrameworkCore;

namespace DevPace.DbConnectionSQLite
{
    public interface IDatabaseConnector
    {
        public DbSet<Models.Customer> Customers { get; set; }

        int SaveChanges();
    }
}
