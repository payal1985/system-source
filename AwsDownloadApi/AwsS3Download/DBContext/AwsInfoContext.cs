using AwsS3Download.DBModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace AwsS3Download.DBContext
{
    public class AwsInfoContext : DbContext
    {
        public AwsInfoContext(DbContextOptions<AwsInfoContext> options) : base(options)
        {
        }

        public DbSet<AwsInfo> awsInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AwsInfo>().ToTable("AwsInfo");
        }
    }
}
