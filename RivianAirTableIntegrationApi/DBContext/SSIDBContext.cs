using Microsoft.EntityFrameworkCore;
using RivianAirtableIntegrationApi.DBModels;
using System.Collections.Generic;
using System.Net.Mail;
using System.Reflection.Emit;

namespace RivianAirtableIntegrationApi.DBContext
{
    public class SSIDBContext : DbContext
    {
        public SSIDBContext(DbContextOptions<SSIDBContext> options) : base(options)
        {
        }

        public DbSet<Requests> Requests { get; set; }   
        public DbSet<Users> Users { get; set; }   
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Requests>().ToTable("requests");
            modelBuilder.Entity<Requests>().ToTable(tb => tb.HasTrigger("tr_requests_client_au"));
            modelBuilder.Entity<Users>().ToTable("users");

        }
    }
}
