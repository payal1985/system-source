using Microsoft.EntityFrameworkCore;
using RequestMilestoneAPI.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestMilestoneAPI.DBContext
{
    public class MilestoneContext : DbContext
    {
        public MilestoneContext(DbContextOptions<MilestoneContext> options) : base(options)
        {
        }

        public DbSet<ClientDateSettings> ClientDateSettings { get; set; }
        public DbSet<ClientRequestDates> ClientRequestDates { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.Entity<Inventory>().HasMany(s => s.InventoryItems).WithOne(s => s.Inventory).HasForeignKey(fk => fk.inventory_id);
            //modelBuilder.Entity<InventoryItem>().HasMany(s => s.InventoryItemImages).WithOne(s => s.InventoryItem).HasForeignKey(fk => fk.inv_item_id);

            modelBuilder.Entity<ClientDateSettings>().ToTable("client_date_settings");
            modelBuilder.Entity<ClientRequestDates>().ToTable("client_request_dates");


        }
    }
}
