﻿using Microsoft.EntityFrameworkCore;
using SupportUI.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportUI.DBContext
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<LeadTime> LeadTimes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("Microsoft_poc_product");
            modelBuilder.Entity<Category>().ToTable("Microsoft_poc_category");
            modelBuilder.Entity<SubCategory>().ToTable("Microsoft_poc_subcategory");
            modelBuilder.Entity<LeadTime>().ToTable("lead_time").HasNoKey();
        }
    }
}