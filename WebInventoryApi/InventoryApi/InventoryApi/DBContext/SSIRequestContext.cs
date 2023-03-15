﻿using Microsoft.EntityFrameworkCore;
using InventoryApi.DBModels.SSIDBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.DBContext
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
        public DbSet<ClientLocations> ClientLocations { get; set; }
        public DbSet<RequestFollowup> RequestFollowups { get; set; }
        public DbSet<RequestFollowupAttachment> RequestFollowupAttachment { get; set; }
        public DbSet<Comapny> Comapnies { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Requests>().ToTable("requests");
            modelBuilder.Entity<Attachments>().ToTable("attachments");
            modelBuilder.Entity<RequestAttachment>().ToTable("rq_attachment");
            modelBuilder.Entity<EmailQueue>().ToTable("email_queue");
            modelBuilder.Entity<ClientSettings>().ToTable("client_settings");
            modelBuilder.Entity<Users>().ToTable("users");
            modelBuilder.Entity<ClientLocations>().ToTable("client_locations");
            modelBuilder.Entity<RequestFollowup>().ToTable("request_followup");
            //modelBuilder.Entity<RequestFollowupAttachment>().ToTable("rqfu_attachment");
            modelBuilder.Entity<RequestFollowupAttachment>()
             //.HasIndex(rqfu=>rqfu.attachment_id).IsUnique();
             .HasKey(rqfu => new { rqfu.request_followup_id, rqfu.attachment_id });

            //.IsUnique();

            modelBuilder.Entity<Comapny>().HasNoKey();//.ToTable("company");

        }
    }
}
