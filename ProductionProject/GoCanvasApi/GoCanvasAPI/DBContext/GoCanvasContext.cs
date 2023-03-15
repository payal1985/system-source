using GoCanvasAPI.DBModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoCanvasAPI.DBContext
{
    public class GoCanvasContext : DbContext
    {
        public GoCanvasContext(DbContextOptions<GoCanvasContext> options) : base(options)
        {
            //Database.SetCommandTimeout(9000);
        }

        public DbSet<FormModel> FormModels { get; set; }
        public DbSet<SubmissionModel> SubmissionModels { get; set; }
        public DbSet<Submission_Section1Model> Submission_Section1Models { get; set; }
        public DbSet<Submission_Section2Model> Submission_Section2Models { get; set; }
        public DbSet<Submission_Section2_ResourceGroupModel> Submission_Section2_ResourceGroupModels { get; set; }
        public DbSet<ImageDataModel> ImageDataModels { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<List<FormModel>>().HasNoKey();
            modelBuilder.Entity<FormModel>().ToTable("FormData");

            modelBuilder.Entity<SubmissionModel>().ToTable("SubmissionsData");        
           
            //modelBuilder.Entity<Submission_Section1Model>().ToTable("Submission_Section1").HasOne<SubmissionModel>(sm=>sm.submissionModel).WithMany(ssm=>ssm.submission_Section1Model).HasForeignKey(sm=>sm.SubmissionId);
            //modelBuilder.Entity<Submission_Section1Model>().ToTable("Submission_Section2").HasOne<SubmissionModel>(sm=>sm.submissionModel).WithMany(ssm=>ssm.submission_Section2Model).HasForeignKey(sm=>sm.SubmissionId);
            //modelBuilder.Entity<Submission_Section1Model>().ToTable("Submission_Section2_ResourceGroup").HasOne<SubmissionModel>(sm=>sm.submissionModel).WithMany(ssm=>ssm.Submission_Section2_ResourceGroupModel).HasForeignKey(sm=>sm.SubmissionId);
            //modelBuilder.Entity<Submission_Section1Model>().ToTable("ImageData").HasOne<SubmissionModel>(sm=>sm.submissionModel).WithMany(ssm=>ssm.ImageDataModel).HasForeignKey(sm=>sm.SubmissionId);

            modelBuilder.Entity<Submission_Section1Model>().ToTable("Submission_Section1");
            modelBuilder.Entity<Submission_Section2Model>().ToTable("Submission_Section2");
            modelBuilder.Entity<Submission_Section2_ResourceGroupModel>().ToTable("Submission_Section2_ResourceGroup");
            modelBuilder.Entity<ImageDataModel>().ToTable("ImageData");

            //modelBuilder.Entity<Submission_Section1Model>(builder =>
            //{
            //    builder.HasNoKey();
            //    builder.ToTable("Submission_Section1");
            //});
            //base.OnModelCreating(modelBuilder);
        }
    }
}
