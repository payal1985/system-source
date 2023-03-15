using System;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace SSInventory.Core.Models
{
    public partial class SSInventoryDbContext : DbContext
    {
        public SSInventoryDbContext(DbContextOptions<SSInventoryDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Inventory> Inventory { get; set; }
        //public virtual DbSet<InventoryBuildings> InventoryBuildings { get; set; }
        public virtual DbSet<InventoryFloors> InventoryFloors { get; set; }
        public virtual DbSet<InventoryImages> InventoryImages { get; set; }
        public virtual DbSet<InventoryItem> InventoryItem { get; set; }
        public virtual DbSet<InventoryItemCondition> InventoryItemCondition { get; set; }
        public virtual DbSet<ItemTypeAttribute> ItemTypeAttribute { get; set; }
        public virtual DbSet<ItemTypeOptionLines> ItemTypeOptionLines { get; set; }
        public virtual DbSet<ItemTypeOptions> ItemTypeOptions { get; set; }
        public virtual DbSet<ItemTypeSupportFiles> ItemTypeSupportFiles { get; set; }
        public virtual DbSet<ItemTypes> ItemTypes { get; set; }
        public virtual DbSet<Manufacturers> Manufacturers { get; set; }
        public virtual DbSet<RefreshTokens> RefreshTokens { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Submissions> Submissions { get; set; }
        public virtual DbSet<UserRoles> UserRoles { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderItem> OrderItem { get; set; }
        public virtual DbSet<HistoryTable> HistoryTables { get; set; }
        public virtual DbSet<InventoryHistory> InventoryHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.ClientId).HasColumnName("client_id");

                entity.Property(e => e.ClientName)
                    .HasMaxLength(500)
                    .HasColumnName("client_name");

                entity.Property(e => e.HasInventory).HasColumnName("has_inventory");

                entity.Property(e => e.SsidbClientId).HasColumnName("ssidb_client_id");

                entity.Property(e => e.TeamdesignCustNo).HasColumnName("teamdesign_cust_no");
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.Property(e => e.InventoryId).HasColumnName("InventoryID");

                entity.Property(e => e.Back)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.BarCode)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Base)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.CreateDateTime).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreateId).HasColumnName("CreateID");

                entity.Property(e => e.Depth).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Diameter).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Edge)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Fabric)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Fabric2)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Finish)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Finish2)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Frame)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.GlobalProductId)
                    .HasColumnName("GlobalProductID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Height).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.ItemCode)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ItemRowId).HasColumnName("ItemRowID");

                entity.Property(e => e.ItemTypeId).HasColumnName("ItemTypeID");

                entity.Property(e => e.MainImage)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ManufacturerId).HasColumnName("ManufacturerID");

                entity.Property(e => e.ManufacturerName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Modular)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PartNumber)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Rfidcode)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("RFIDCode")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Seat)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SeatHeight)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Size)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SubmissionId)
                    .HasColumnName("SubmissionID")
                    .HasDefaultValueSql("(0)");

                entity.Property(e => e.Tag).HasMaxLength(25);

                entity.Property(e => e.Top)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UpdateId).HasColumnName("UpdateID");

                entity.Property(e => e.Width).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.QRCode).HasMaxLength(200);

                entity.Property(e => e.WarrantyYears).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.ItemType)
                    .WithMany(p => p.Inventory)
                    .HasForeignKey(d => d.ItemTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inventory_ItemTypes");
            });
            /*
            modelBuilder.Entity<InventoryBuildings>(entity =>
            {
                entity.HasKey(e => e.InventoryBuildingId);

                entity.Property(e => e.InventoryBuildingId).HasColumnName("InventoryBuildingID");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.CreateDateTime).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreateId).HasColumnName("CreateID");

                entity.Property(e => e.InventoryBuildingCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InventoryBuildingDesc)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.InventoryBuildingName)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.StatusId)
                    .HasColumnName("StatusID")
                    .HasDefaultValueSql("((5))");

                entity.Property(e => e.UpdateId).HasColumnName("UpdateID");
            });
            */
            modelBuilder.Entity<InventoryFloors>(entity =>
            {
                entity.HasKey(e => e.InventoryFloorId);

                entity.Property(e => e.InventoryFloorId).HasColumnName("InventoryFloorID");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.CreateDateTime).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreateId).HasColumnName("CreateID");

                entity.Property(e => e.InventoryFloorCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.InventoryFloorDesc)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.InventoryFloorName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.StatusId)
                    .HasColumnName("StatusID")
                    .HasDefaultValueSql("((5))");

                entity.Property(e => e.UpdateId).HasColumnName("UpdateID");
            });

            modelBuilder.Entity<InventoryImages>(entity =>
            {
                entity.HasKey(e => e.InventoryImageId)
                    .HasName("PK_InventoryItemImages");

                entity.Property(e => e.InventoryImageId).HasColumnName("InventoryImageID");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.CreateDateTime).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreateId).HasColumnName("CreateID");

                entity.Property(e => e.ImageName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ImageUrl)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.InventoryId).HasColumnName("InventoryID");

                entity.Property(e => e.ConditionId).HasColumnName("ConditionID");

                entity.Property(e => e.InventoryItemId).HasColumnName("InventoryItemID");

                entity.Property(e => e.ImageGuid).HasDefaultValueSql("(newid())");

                entity.Property(e => e.ItemTypeAutomationId).HasColumnName("ItemTypeAutomationID");

                entity.Property(e => e.ItemTypeAutomationOptionId).HasColumnName("ItemTypeAutomationOptionID");

                entity.Property(e => e.SubmissionDate).HasColumnType("datetime");

                entity.Property(e => e.TempPhotoName)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UpdateId).HasColumnName("UpdateID");
            });

            modelBuilder.Entity<InventoryItem>(entity =>
            {
                entity.Property(e => e.InventoryItemId).HasColumnName("InventoryItemID");

                entity.Property(e => e.Barcode)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ClientId)
                    .HasColumnName("ClientID")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CreateDateTime).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreateId).HasColumnName("CreateID");

                entity.Property(e => e.DamageNotes)
                    .IsRequired()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DisplayOnSite)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Gpslocation)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("GPSLocation")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.InventoryBuildingId).HasColumnName("InventoryBuildingID");

                entity.Property(e => e.InventoryFloorId).HasColumnName("InventoryFloorID");

                entity.Property(e => e.InventoryId).HasColumnName("InventoryID");

                entity.Property(e => e.InventoryOwnerId).HasColumnName("InventoryOwnerID");

                entity.Property(e => e.InventorySpaceTypeId).HasColumnName("InventorySpaceTypeID");

                entity.Property(e => e.NonSsipurchaseDate).HasColumnName("NonSSIPurchaseDate");

                entity.Property(e => e.PoOrderNo).HasColumnType("numeric(8, 0)");

                entity.Property(e => e.ProposalNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Qrcode)
                    .HasMaxLength(200)
                    .HasColumnName("QRCode")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Rfidcode)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("RFIDcode")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Room)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.StatusId)
                    .HasColumnName("StatusID")
                    .HasDefaultValueSql("((5))");

                entity.Property(e => e.SubmissionId)
                    //.IsRequired()
                    .HasColumnName("SubmissionID")
                    .HasDefaultValueSql("(0)");

                entity.Property(e => e.UpdateId).HasColumnName("UpdateID");

                entity.Property(e => e.ConditionId).HasColumnName("ConditionID");

                entity.Property(e => e.WarrantyRequestId).HasColumnName("WarrantyRequestID");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.AddedToCartItem).IsRequired()
                .HasDefaultValue(false);

                //entity.HasOne(d => d.InventoryBuilding)
                //    .WithMany(p => p.InventoryItem)
                //    .HasForeignKey(d => d.InventoryBuildingId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_InventoryItem_Building");

                entity.HasOne(d => d.InventoryFloor)
                    .WithMany(p => p.InventoryItem)
                    .HasForeignKey(d => d.InventoryFloorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InventoryItem_Floor");

                entity.HasOne(d => d.Inventory)
                    .WithMany(p => p.InventoryItem)
                    .HasForeignKey(d => d.InventoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InventoryItem_Inventory");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.InventoryItem)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InventoryItem_Status");

            });

            modelBuilder.Entity<InventoryItemCondition>(entity =>
            {
                entity.Property(e => e.InventoryItemConditionId).HasColumnName("InventoryItemConditionID");

                entity.Property(e => e.ConditionName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDateTime).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreateId).HasColumnName("CreateID");

                entity.Property(e => e.StatusId)
                    .HasColumnName("StatusID")
                    .HasDefaultValueSql("((5))");

                entity.Property(e => e.IsMobileCondition)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(e => e.UpdateId).HasColumnName("UpdateID");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.InventoryItemCondition)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InventoryItemCondition_Status");
            });

            modelBuilder.Entity<ItemTypeAttribute>(entity =>
            {
                entity.Property(e => e.ItemTypeAttributeId).HasColumnName("ItemTypeAttributeID");

                entity.Property(e => e.CreateDateTime).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreateId).HasColumnName("CreateID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateId).HasColumnName("UpdateID");
            });

            modelBuilder.Entity<ItemTypeOptionLines>(entity =>
            {
                entity.HasKey(e => e.ItemTypeOptionLineId);

                entity.Property(e => e.ItemTypeOptionLineId).HasColumnName("ItemTypeOptionLineID");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.CreateDateTime).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreateId).HasColumnName("CreateID");

                entity.Property(e => e.InventoryUserAcceptanceRulesRequired)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ItemTypeOptionId).HasColumnName("ItemTypeOptionID");

                entity.Property(e => e.ItemTypeOptionLineCode)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ItemTypeOptionLineDesc)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ItemTypeOptionLineName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.StatusId)
                    .HasColumnName("StatusID")
                    .HasDefaultValueSql("((5))");

                entity.Property(e => e.UpdateId).HasColumnName("UpdateID");

                entity.HasOne(d => d.ItemTypeOption)
                    .WithMany(p => p.ItemTypeOptionLines)
                    .HasForeignKey(d => d.ItemTypeOptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemTypeOptionLines_ItemTypeOptions");
            });

            modelBuilder.Entity<ItemTypeOptions>(entity =>
            {
                entity.HasKey(e => e.ItemTypeOptionId);

                entity.Property(e => e.ItemTypeOptionId).HasColumnName("ItemTypeOptionID");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.CreateDateTime).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreateId).HasColumnName("CreateID");

                entity.Property(e => e.FieldType)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ItemTypeAttributeId).HasColumnName("ItemTypeAttributeID");

                entity.Property(e => e.ItemTypeId).HasColumnName("ItemTypeID");

                entity.Property(e => e.ItemTypeOptionCode)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ItemTypeOptionDesc)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ItemTypeOptionName)
                   .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ItemTypeSupportFileId).HasColumnName("ItemTypeSupportFileID");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.UpdateId).HasColumnName("UpdateID");

                entity.HasOne(d => d.ItemType)
                    .WithMany(p => p.ItemTypeOptions)
                    .HasForeignKey(d => d.ItemTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemTypeOptions_ItemTypes");
            });

            modelBuilder.Entity<ItemTypeSupportFiles>(entity =>
            {
                entity.HasKey(e => e.ItemTypeSupportFileId)
                    .HasName("PK_ItemTypeSupportFile");

                entity.Property(e => e.ItemTypeSupportFileId).HasColumnName("ItemTypeSupportFileID");

                entity.Property(e => e.CreateDateTime).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreateId).HasColumnName("CreateID");

                entity.Property(e => e.Desc)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FilePath)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateId).HasColumnName("UpdateID");
            });

            modelBuilder.Entity<ItemTypes>(entity =>
            {
                entity.HasKey(e => e.ItemTypeId);

                entity.Property(e => e.ItemTypeId).HasColumnName("ItemTypeID");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.CreateDateTime).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreateId).HasColumnName("CreateID");

                entity.Property(e => e.ItemTypeAttributeId).HasColumnName("ItemTypeAttributeID");

                entity.Property(e => e.ItemTypeCode)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ItemTypeDesc)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ItemTypeName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UpdateId).HasColumnName("UpdateID");

                entity.HasOne(d => d.ItemTypeAttribute)
                    .WithMany(p => p.ItemTypes)
                    .HasForeignKey(d => d.ItemTypeAttributeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemTypes_ItemTypeAttribute");
            });

            modelBuilder.Entity<Manufacturers>(entity =>
            {
                entity.HasKey(e => e.ManufacturerId);

                entity.Property(e => e.ManufacturerId).HasColumnName("ManufacturerID");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CreateDateTime).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreateId).HasColumnName("CreateID");

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IsDsrenabled).HasColumnName("IsDSREnabled");

                entity.Property(e => e.ManufacturerName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Pmtype)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PMType")
                    .HasDefaultValueSql("('')")
                    .IsFixedLength(true);

                entity.Property(e => e.SsimanufacturerId).HasColumnName("SSIManufacturerID");

                entity.Property(e => e.SsimanufacturerName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SSIManufacturerName")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UpdateId).HasColumnName("UpdateID");

                entity.Property(e => e.VendorNum)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<RefreshTokens>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.CreatedByIp).HasMaxLength(50);

                entity.Property(e => e.Expires).HasColumnType("datetime");

                entity.Property(e => e.ReplacedByToken).HasMaxLength(200);

                entity.Property(e => e.Revoked).HasColumnType("datetime");

                entity.Property(e => e.RevokedByIp).HasMaxLength(50);

                entity.Property(e => e.Token).HasMaxLength(200);
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__Roles__8AFACE3A6AB38702");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.LastModDate).HasColumnType("datetime");

                entity.Property(e => e.RoleDescription)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.RoleName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StatusId).HasColumnName("StatusID");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.CreateDateTime).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreateId).HasColumnName("CreateID");

                entity.Property(e => e.StatusDesc)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StatusName)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.StatusType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UpdateId).HasColumnName("UpdateID");
            });

            modelBuilder.Entity<Submissions>(entity =>
            {
                entity.HasKey(e => e.SubmissionId)
                    .HasName("PK_Submitions");

                entity.Property(e => e.Client).HasMaxLength(250);

                entity.Property(e => e.CreateDateTime).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreateId).HasColumnName("CreateID");

                entity.Property(e => e.DeviceDate).HasMaxLength(50);

                entity.Property(e => e.InventoryAppId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasMaxLength(100);

                entity.Property(e => e.TempColumnJsonForTesting).IsUnicode(false);

                entity.Property(e => e.UpdateId).HasColumnName("UpdateID");
            });

            modelBuilder.Entity<UserRoles>(entity =>
            {
                entity.HasKey(e => e.UserRoleId)
                    .HasName("PK__UserRole__3D978A5584CBDC00");

                entity.Property(e => e.UserRoleId).HasColumnName("UserRoleID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LastModDate).HasColumnType("datetime");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__Users__1788CCAC1CE856C5");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.CellPhone)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LastModDate).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordEncrypted)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PreferLanguage)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.UserFname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("UserFName");

                entity.Property(e => e.UserLname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("UserLName");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserRoleId).HasColumnName("UserRoleID");

                entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");

                entity.Property(e => e.WorkPhone)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderID)
                    .HasName("PK_Order");

                entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(50);

                entity.Property(e => e.Project)
                .IsRequired()
                .HasMaxLength(50);

                entity.Property(e => e.StatusID)
                .IsRequired();

                entity.Property(e => e.StatusID)
                .IsRequired();
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.OrderItemID)
                    .HasName("PK_OrderItem");

                entity.Property(e => e.OrderID)
                .IsRequired();

                entity.Property(e => e.InventoryItemID)
                .IsRequired();

                entity.Property(e => e.InventoryID)
                .IsRequired();

                entity.Property(e => e.ItemCode)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Room)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DepartmentCostCenter)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DestFloorID)
                .IsRequired();

                entity.Property(e => e.DepartmentCostCenter)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.InstallDate)
                    .IsRequired();

                entity.Property(e => e.Delivered)
                    .IsRequired();

                entity.Property(e => e.Completed)
                    .IsRequired();

                entity.Property(e => e.Comments)
                    .IsRequired();

                entity.Property(e => e.Qty)
                    .IsRequired();
            });

            modelBuilder.Entity<HistoryTable>(entity =>
            {
                entity.ToTable("HistoryTables");
                entity.HasKey(e => e.HistoryTableId).HasName("HistoryTableID");
                entity.Property(e => e.EntityId).HasColumnName("EntityID").IsRequired();
                entity.Property(e => e.TableName).HasMaxLength(50).IsRequired();
                entity.Property(e => e.BatchTransactionGUID).HasMaxLength(50);
                entity.Property(e => e.CreateDateTime).HasColumnName("CreateDateTime").IsRequired().HasDefaultValue(DateTime.Now);
                entity.Property(e => e.CreateId).HasColumnName("CreateID").IsRequired().HasDefaultValue(1);
                entity.Property(e => e.OrderId).HasColumnName("OrderID");
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}