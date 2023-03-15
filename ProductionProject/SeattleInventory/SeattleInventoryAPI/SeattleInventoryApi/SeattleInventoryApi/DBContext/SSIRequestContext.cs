using Microsoft.EntityFrameworkCore;
using SeattleInventoryApi.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.DBContext
{
    public class SSIRequestContext : DbContext
    {
        public SSIRequestContext(DbContextOptions<SSIRequestContext> options) : base(options)
        {
        }

        public DbSet<Requests> Requests { get; set; }
        public DbSet<Attachments> Attachments { get; set; }
        public DbSet<RequestAttachment> RequestAttachments { get; set; }
        public DbSet<EmailQueue> EmailQueues { get; set; }
        public DbSet<ClientSettings> ClientSettings { get; set; }
        public DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Requests>().ToTable("requests");
            modelBuilder.Entity<Attachments>().ToTable("attachments");
            modelBuilder.Entity<RequestAttachment>().ToTable("rq_attachment");
            modelBuilder.Entity<EmailQueue>().ToTable("email_queue");
            modelBuilder.Entity<ClientSettings>().ToTable("client_settings");
            modelBuilder.Entity<Users>().ToTable("users");
        }
    }
}
