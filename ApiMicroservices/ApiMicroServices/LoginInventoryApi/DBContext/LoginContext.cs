using LoginInventoryApi.DBModels;
using Microsoft.EntityFrameworkCore;

namespace LoginInventoryApi.DBContext
{
    public class LoginContext : DbContext
    {

        public LoginContext(DbContextOptions<LoginContext> options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Clients> Clients { get; set; }
        public DbSet<UserTypes> UserTypes { get; set; }
        public DbSet<UserPermissions> UserPermissions { get; set; }
        public DbSet<ClientLocations> ClientLocations { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<StateProvince> StateProvinces { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().ToTable("users");
            modelBuilder.Entity<Clients>().ToTable("clients");
            modelBuilder.Entity<UserTypes>().ToTable("user_type");
            modelBuilder.Entity<UserPermissions>().HasKey(x => new { x.client_id, x.user_id });
            modelBuilder.Entity<ClientLocations>().ToTable("client_locations");
            modelBuilder.Entity<Country>().ToTable("countries");
            modelBuilder.Entity<StateProvince>().ToTable("state_province");
        }
    }
}