using Microsoft.EntityFrameworkCore;
using SSInventory.Core.Models;
using System.Data.Common;

namespace SSInventory.Core.InventoryContext
{
    public class DatabaseContext : SSInventoryDbContext
    {
        private DbConnection _connection;

        public DatabaseContext() : base() { }

        public DatabaseContext(DbConnection connection) : base()
        {
            _connection = connection;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_connection != null)
            {
                optionsBuilder.UseSqlServer(_connection);
            }
            else
            {
                optionsBuilder.UseSqlServer(@"Data Source=localhost;Initial Catalog=SSDB; user id=sa;password=Love19861987;");
            }
        }
    }
}
