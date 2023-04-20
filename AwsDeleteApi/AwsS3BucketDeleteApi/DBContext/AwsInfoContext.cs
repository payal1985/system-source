﻿using AwsS3BucketDeleteApi.DBModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsS3BucketDeleteApi.DBContext
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