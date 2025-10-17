    using System;
    using System.Collections.Generic;
    using BanDTh.Models;
    using Microsoft.EntityFrameworkCore;

    namespace BanDTh.Data;

    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Brand> Brands { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<CustomerFeedback> CustomerFeedbacks { get; set; }

        public virtual DbSet<DiscountCode> DiscountCodes { get; set; }

        public virtual DbSet<EmployeeKpi> EmployeeKpis { get; set; }

        public virtual DbSet<InventoryStatistic> InventoryStatistics { get; set; }

        public virtual DbSet<News> News { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderDetail> OrderDetails { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Promotion> Promotions { get; set; }

        public virtual DbSet<RepairAccessory> RepairAccessories { get; set; }

        public virtual DbSet<RepairCategory> RepairCategories { get; set; }

        public virtual DbSet<RepairItem> RepairItems { get; set; }

        public virtual DbSet<RewardHistory> RewardHistories { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<SalesStatistic> SalesStatistics { get; set; }

        public virtual DbSet<SystemLog> SystemLogs { get; set; }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Name=AppDb");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>(entity =>
            {
                entity.HasKey(e => e.BrandId).HasName("PK__Brands__DAD4F05E343A70DA");

                entity.HasIndex(e => e.Slug, "UQ__Brands__BC7B5FB6F349E3DE").IsUnique();

                entity.Property(e => e.Logo).HasMaxLength(255);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Slug).HasMaxLength(100);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0B8CDC340D");

                entity.HasIndex(e => e.Slug, "UQ__Categori__BC7B5FB6AD26238E").IsUnique();

                entity.Property(e => e.Icon).HasMaxLength(255);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Slug).HasMaxLength(100);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D8982A91BE");

                entity.Property(e => e.Address).HasMaxLength(255);
                entity.Property(e => e.City).HasMaxLength(100);
                entity.Property(e => e.PostalCode).HasMaxLength(20);
                entity.Property(e => e.Province).HasMaxLength(100);

                entity.HasOne(d => d.User).WithMany(p => p.Customers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Customers__UserI__3DE82FB7");
            });

            modelBuilder.Entity<CustomerFeedback>(entity =>
            {
                entity.HasKey(e => e.FeedbackId).HasName("PK__Customer__6A4BEDD6503CC0E5");

                entity.Property(e => e.Comment).HasMaxLength(500);
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Order).WithMany(p => p.CustomerFeedbacks)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CustomerF__Order__42ACE4D4");

                entity.HasOne(d => d.User).WithMany(p => p.CustomerFeedbacks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CustomerF__UserI__43A1090D");
            });

            modelBuilder.Entity<DiscountCode>(entity =>
            {
                entity.HasKey(e => e.DiscountId).HasName("PK__Discount__E43F6D96AD5D626C");

                entity.HasIndex(e => e.Code, "UQ__Discount__A25C5AA7874A4707").IsUnique();

                entity.Property(e => e.Code).HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.DiscountType)
                    .HasMaxLength(20)
                    .HasDefaultValue("Percent");
                entity.Property(e => e.DiscountValue).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
                entity.Property(e => e.MaxDiscount).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.MinOrderValue)
                    .HasDefaultValue(0m)
                    .HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .HasDefaultValue("Hoạt động");
            });

            modelBuilder.Entity<EmployeeKpi>(entity =>
            {
                entity.HasKey(e => e.Kpiid).HasName("PK__Employee__72E692A1B9828887");

                entity.ToTable("EmployeeKPI");

                entity.Property(e => e.Kpiid).HasColumnName("KPIId");
                entity.Property(e => e.Bonus)
                    .HasDefaultValue(0m)
                    .HasColumnType("decimal(18, 2)");
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.CustomerCount).HasDefaultValue(0);
                entity.Property(e => e.SatisfactionScore)
                    .HasDefaultValue(0m)
                    .HasColumnType("decimal(5, 2)");
                entity.Property(e => e.TotalOrders).HasDefaultValue(0);
                entity.Property(e => e.TotalRevenue)
                    .HasDefaultValue(0m)
                    .HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.User).WithMany(p => p.EmployeeKpis)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EmployeeK__UserI__36470DEF");
            });

            modelBuilder.Entity<InventoryStatistic>(entity =>
            {
                entity.HasKey(e => e.InventoryId).HasName("PK__Inventor__F5FDE6B33113DF8E");

                entity.Property(e => e.Note).HasMaxLength(255);
                entity.Property(e => e.QuantityIn).HasDefaultValue(0);
                entity.Property(e => e.QuantityOut).HasDefaultValue(0);
                entity.Property(e => e.StockRemaining).HasDefaultValue(0);
                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Product).WithMany(p => p.InventoryStatistics)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Inventory__Produ__2CBDA3B5");

                entity.HasOne(d => d.User).WithMany(p => p.InventoryStatistics)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Inventory__UserI__2DB1C7EE");
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.HasKey(e => e.NewsId).HasName("PK__News__954EBDF3F45BE144");

                entity.HasIndex(e => e.Slug, "UQ__News__BC7B5FB6BC948C25").IsUnique();

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Slug).HasMaxLength(150);
                entity.Property(e => e.Thumbnail).HasMaxLength(255);
                entity.Property(e => e.Title).HasMaxLength(200);

                entity.HasOne(d => d.Author).WithMany(p => p.News)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__News__AuthorId__01D345B0");

                entity.HasOne(d => d.Category).WithMany(p => p.News)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__News__CategoryId__02C769E9");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCFE7084B8D");

                entity.ToTable(tb => tb.HasTrigger("trg_UpdateStats_OnOrder"));

                entity.HasIndex(e => e.OrderCode, "UQ__Orders__999B522918E167C6").IsUnique();

                entity.Property(e => e.Address).HasMaxLength(255);
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Instruction).HasDefaultValue(false);
                entity.Property(e => e.Note).HasMaxLength(255);
                entity.Property(e => e.OrderCode).HasMaxLength(50);
                entity.Property(e => e.PaymentMethod).HasMaxLength(100);
                entity.Property(e => e.ReceiverName).HasMaxLength(100);
                entity.Property(e => e.ReceiverPhone).HasMaxLength(15);
                entity.Property(e => e.RewardPoints).HasDefaultValue(0);
                entity.Property(e => e.ShippingMethod).HasMaxLength(100);
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasDefaultValue("Chờ xử lý");
                entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.TotalProducts).HasDefaultValue(0);

                entity.HasOne(d => d.Discount).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.DiscountId)
                    .HasConstraintName("FK__Orders__Discount__4865BE2A");

                entity.HasOne(d => d.User).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__UserId__0B5CAFEA");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__D3B9D36C698855F3");

                entity.Property(e => e.Quantity).HasDefaultValue(1);
                entity.Property(e => e.TotalPrice)
                    .HasComputedColumnSql("([Quantity]*[UnitPrice])", true)
                    .HasColumnType("decimal(29, 2)");
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderDeta__Order__3A179ED3");

                entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderDeta__Produ__3B0BC30C");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6CD5A8F413B");

                entity.HasIndex(e => e.Slug, "UQ__Products__BC7B5FB6F069568E").IsUnique();

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Name).HasMaxLength(150);
                entity.Property(e => e.OldPrice).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Slug).HasMaxLength(100);
                entity.Property(e => e.Stock).HasDefaultValue(0);
                entity.Property(e => e.Thumbnail).HasMaxLength(255);

                entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK__Products__BrandI__74794A92");

                entity.HasOne(d => d.Category).WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Products__Catego__73852659");

                entity.HasMany(d => d.Promotions).WithMany(p => p.Products)
                    .UsingEntity<Dictionary<string, object>>(
                        "ProductPromotion",
                        r => r.HasOne<Promotion>().WithMany()
                            .HasForeignKey("PromotionId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK__ProductPr__Promo__18B6AB08"),
                        l => l.HasOne<Product>().WithMany()
                            .HasForeignKey("ProductId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK__ProductPr__Produ__17C286CF"),
                        j =>
                        {
                            j.HasKey("ProductId", "PromotionId").HasName("PK__ProductP__51208431CD18F1A1");
                            j.ToTable("ProductPromotions");
                        });
            });

            modelBuilder.Entity<Promotion>(entity =>
            {
                entity.HasKey(e => e.PromotionId).HasName("PK__Promotio__52C42FCFF46FF448");

                entity.Property(e => e.DiscountType)
                    .HasMaxLength(20)
                    .HasDefaultValue("Percent");
                entity.Property(e => e.DiscountValue).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.EndDate).HasColumnType("datetime");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.StartDate).HasColumnType("datetime");
                entity.Property(e => e.Title).HasMaxLength(150);
            });

            modelBuilder.Entity<RepairAccessory>(entity =>
            {
                entity.HasKey(e => e.RepairAccessoryId).HasName("PK__RepairAc__4F0BBFFFCDD5E8D9");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<RepairCategory>(entity =>
            {
                entity.HasKey(e => e.RepairCategoryId).HasName("PK__RepairCa__4BCB294BC1045B89");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<RepairItem>(entity =>
            {
                entity.HasKey(e => e.RepairItemId).HasName("PK__RepairIt__17083572E0695C2C");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.RepairName).HasMaxLength(150);

                entity.HasOne(d => d.Product).WithMany(p => p.RepairItems)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RepairIte__Produ__7B264821");

                entity.HasOne(d => d.RepairAccessory).WithMany(p => p.RepairItems)
                    .HasForeignKey(d => d.RepairAccessoryId)
                    .HasConstraintName("FK__RepairIte__Repai__7D0E9093");

                entity.HasOne(d => d.RepairCategory).WithMany(p => p.RepairItems)
                    .HasForeignKey(d => d.RepairCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RepairIte__Repai__7C1A6C5A");
            });

            modelBuilder.Entity<RewardHistory>(entity =>
            {
                entity.HasKey(e => e.RewardId).HasName("PK__RewardHi__825015B96B45853A");

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.Type)
                    .HasMaxLength(20)
                    .HasDefaultValue("Earn");

                entity.HasOne(d => d.Order).WithMany(p => p.RewardHistories)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__RewardHis__Order__1E6F845E");

                entity.HasOne(d => d.User).WithMany(p => p.RewardHistories)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RewardHis__UserI__1D7B6025");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1A1F225FB0");

                entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B6160995BEDB1").IsUnique();

                entity.Property(e => e.RoleDescription).HasMaxLength(200);
                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            modelBuilder.Entity<SalesStatistic>(entity =>
            {
                entity.HasKey(e => e.StatisticId).HasName("PK__SalesSta__367DEB17850E5E30");

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.TotalOrders).HasDefaultValue(0);
                entity.Property(e => e.TotalProducts).HasDefaultValue(0);
                entity.Property(e => e.TotalRevenue)
                    .HasDefaultValue(0m)
                    .HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.User).WithMany(p => p.SalesStatistics)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SalesStat__UserI__251C81ED");
            });

            modelBuilder.Entity<SystemLog>(entity =>
            {
                entity.HasKey(e => e.LogId).HasName("PK__SystemLo__5E5486484909D4E5");

                entity.Property(e => e.Action).HasMaxLength(100);
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.TableName).HasMaxLength(100);

                entity.HasOne(d => d.User).WithMany(p => p.SystemLogs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__SystemLog__UserI__477199F1");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C08DA5B21");

                entity.HasIndex(e => e.UserName, "UQ__Users__C9F28456109995C5").IsUnique();

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.FullName).HasMaxLength(100);
                entity.Property(e => e.Gender).HasMaxLength(10);
                entity.Property(e => e.PasswordHash).HasMaxLength(255);
                entity.Property(e => e.Phone).HasMaxLength(15);
                entity.Property(e => e.UserName).HasMaxLength(100);

                entity.HasOne(d => d.Role).WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Users__RoleId__6AEFE058");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
