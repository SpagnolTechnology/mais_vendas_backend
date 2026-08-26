using Domain.Entity;

using Microsoft.EntityFrameworkCore;



namespace Infrastructure.Context

{

    public class DatabaseContext : DbContext

    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        {

            base.OnConfiguring(optionsBuilder);

        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {

            base.OnModelCreating(modelBuilder);



            modelBuilder.Entity<TestEntity>(entity =>

            {

                entity.ToTable("Tests");

                entity.HasKey(x => x.Id);



                entity.Property(x => x.Id).ValueGeneratedOnAdd();

                entity.Property(x => x.Name).HasMaxLength(255).IsRequired();

                entity.Property(x => x.Description).HasMaxLength(1000);

                entity.Property(x => x.IsActive).IsRequired();

                entity.Property(x => x.ReferenceDate).IsRequired();

                entity.Property(x => x.CreatedAt).IsRequired();

                entity.Property(x => x.CreatedBy).HasMaxLength(255).IsRequired();

                entity.Property(x => x.UpdatedAt);

                entity.Property(x => x.UpdatedBy).HasMaxLength(255);

            });



            modelBuilder.Entity<SupplierEntity>(entity =>

            {

                entity.ToTable("Suppliers");

                entity.HasKey(x => x.Id);



                entity.Property(x => x.Id).ValueGeneratedOnAdd();

                entity.Property(x => x.Name).HasMaxLength(255).IsRequired();

                entity.Property(x => x.Document).HasMaxLength(20).IsRequired();

                entity.Property(x => x.Email).HasMaxLength(255);

                entity.Property(x => x.Phone).HasMaxLength(50);

                entity.Property(x => x.Address).HasMaxLength(500);

                entity.Property(x => x.City).HasMaxLength(100);

                entity.Property(x => x.State).HasMaxLength(50);

                entity.Property(x => x.ZipCode).HasMaxLength(20);

                entity.Property(x => x.IsActive).IsRequired();

                entity.Property(x => x.CreatedAt).IsRequired();

                entity.Property(x => x.CreatedBy).HasMaxLength(255).IsRequired();

                entity.Property(x => x.UpdatedAt);

                entity.Property(x => x.UpdatedBy).HasMaxLength(255);

            });



            modelBuilder.Entity<UnitOfMeasureEntity>(entity =>

            {

                entity.ToTable("UnitOfMeasures");

                entity.HasKey(x => x.Id);



                entity.Property(x => x.Id).ValueGeneratedOnAdd();

                entity.Property(x => x.Name).HasMaxLength(255).IsRequired();

                entity.Property(x => x.Abbreviation).HasMaxLength(20).IsRequired();

                entity.Property(x => x.IsActive).IsRequired();

                entity.Property(x => x.CreatedAt).IsRequired();

                entity.Property(x => x.CreatedBy).HasMaxLength(255).IsRequired();

                entity.Property(x => x.UpdatedAt);

                entity.Property(x => x.UpdatedBy).HasMaxLength(255);

            });



            modelBuilder.Entity<ProductEntity>(entity =>

            {

                entity.ToTable("Products");

                entity.HasKey(x => x.Id);



                entity.Property(x => x.Id).ValueGeneratedOnAdd();

                entity.Property(x => x.UnitOfMeasureId).IsRequired();

                entity.Property(x => x.Name).HasMaxLength(255).IsRequired();

                entity.Property(x => x.Sku).HasMaxLength(100).IsRequired();

                entity.Property(x => x.Description).HasMaxLength(1000);

                entity.Property(x => x.UnitPrice).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.CostPrice).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.MarkupPercent).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.IcmsPercent).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.IssPercent).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.PisPercent).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.CofinsPercent).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.IsActive).IsRequired();

                entity.Property(x => x.CreatedAt).IsRequired();

                entity.Property(x => x.CreatedBy).HasMaxLength(255).IsRequired();

                entity.Property(x => x.UpdatedAt);

                entity.Property(x => x.UpdatedBy).HasMaxLength(255);



                entity.HasOne(x => x.UnitOfMeasure)

                    .WithMany()

                    .HasForeignKey(x => x.UnitOfMeasureId)

                    .OnDelete(DeleteBehavior.Restrict);



                entity.HasOne(x => x.ProductStock)

                    .WithOne(x => x.Product)

                    .HasForeignKey<ProductStockEntity>(x => x.ProductId)

                    .OnDelete(DeleteBehavior.Cascade);

            });



            modelBuilder.Entity<ProductStockEntity>(entity =>

            {

                entity.ToTable("ProductStocks");

                entity.HasKey(x => x.Id);



                entity.Property(x => x.Id).ValueGeneratedOnAdd();

                entity.Property(x => x.ProductId).IsRequired();

                entity.Property(x => x.Quantity).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.MinimumQuantity).HasPrecision(18, 4);

                entity.Property(x => x.CreatedAt).IsRequired();

                entity.Property(x => x.CreatedBy).HasMaxLength(255).IsRequired();

                entity.Property(x => x.UpdatedAt);

                entity.Property(x => x.UpdatedBy).HasMaxLength(255);



                entity.HasIndex(x => x.ProductId).IsUnique();

            });



            modelBuilder.Entity<PaymentConditionEntity>(entity =>

            {

                entity.ToTable("PaymentConditions");

                entity.HasKey(x => x.Id);



                entity.Property(x => x.Id).ValueGeneratedOnAdd();

                entity.Property(x => x.Name).HasMaxLength(255).IsRequired();

                entity.Property(x => x.InstallmentCount).IsRequired();

                entity.Property(x => x.DaysUntilFirstDue).IsRequired();

                entity.Property(x => x.DaysBetweenInstallments).IsRequired();

                entity.Property(x => x.CashDiscountPercent).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.IsActive).IsRequired();

                entity.Property(x => x.CreatedAt).IsRequired();

                entity.Property(x => x.CreatedBy).HasMaxLength(255).IsRequired();

                entity.Property(x => x.UpdatedAt);

                entity.Property(x => x.UpdatedBy).HasMaxLength(255);

            });



            modelBuilder.Entity<DiscountRuleEntity>(entity =>

            {

                entity.ToTable("DiscountRules");

                entity.HasKey(x => x.Id);



                entity.Property(x => x.Id).ValueGeneratedOnAdd();

                entity.Property(x => x.Role).HasMaxLength(255);

                entity.Property(x => x.UserEmail).HasMaxLength(255);

                entity.Property(x => x.MaxDiscountPercent).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.MaxDiscountAmount).HasPrecision(18, 4);

                entity.Property(x => x.IsActive).IsRequired();

                entity.Property(x => x.CreatedAt).IsRequired();

                entity.Property(x => x.CreatedBy).HasMaxLength(255).IsRequired();

                entity.Property(x => x.UpdatedAt);

                entity.Property(x => x.UpdatedBy).HasMaxLength(255);

            });



            modelBuilder.Entity<CommissionRuleEntity>(entity =>

            {

                entity.ToTable("CommissionRules");

                entity.HasKey(x => x.Id);



                entity.Property(x => x.Id).ValueGeneratedOnAdd();

                entity.Property(x => x.ProductId);

                entity.Property(x => x.CalculationScope);

                entity.Property(x => x.CommissionPercent).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.IsActive).IsRequired();

                entity.Property(x => x.CreatedAt).IsRequired();

                entity.Property(x => x.CreatedBy).HasMaxLength(255).IsRequired();

                entity.Property(x => x.UpdatedAt);

                entity.Property(x => x.UpdatedBy).HasMaxLength(255);



                entity.HasOne(x => x.Product)

                    .WithMany()

                    .HasForeignKey(x => x.ProductId)

                    .OnDelete(DeleteBehavior.SetNull);

            });



            modelBuilder.Entity<DocumentSequenceEntity>(entity =>

            {

                entity.ToTable("DocumentSequences");

                entity.HasKey(x => x.Id);



                entity.Property(x => x.Id).ValueGeneratedOnAdd();

                entity.Property(x => x.DocumentType).IsRequired();

                entity.Property(x => x.Year).IsRequired();

                entity.Property(x => x.LastNumber).IsRequired();

                entity.Property(x => x.CreatedAt).IsRequired();

                entity.Property(x => x.CreatedBy).HasMaxLength(255).IsRequired();

                entity.Property(x => x.UpdatedAt);

                entity.Property(x => x.UpdatedBy).HasMaxLength(255);



                entity.HasIndex(x => new { x.DocumentType, x.Year }).IsUnique();

            });



            modelBuilder.Entity<ProductPurchaseEntryEntity>(entity =>

            {

                entity.ToTable("ProductPurchaseEntries");

                entity.HasKey(x => x.Id);



                entity.Property(x => x.Id).ValueGeneratedOnAdd();

                entity.Property(x => x.Number).HasMaxLength(20).IsRequired();

                entity.Property(x => x.SupplierId).IsRequired();

                entity.Property(x => x.InvoiceNumber).HasMaxLength(50).IsRequired();

                entity.Property(x => x.InvoiceSeries).HasMaxLength(10).IsRequired();

                entity.Property(x => x.InvoiceKey).HasMaxLength(44);

                entity.Property(x => x.Status).IsRequired();

                entity.Property(x => x.EntryDate).IsRequired();

                entity.Property(x => x.Notes).HasMaxLength(1000);

                entity.Property(x => x.CreatedAt).IsRequired();

                entity.Property(x => x.CreatedBy).HasMaxLength(255).IsRequired();

                entity.Property(x => x.UpdatedAt);

                entity.Property(x => x.UpdatedBy).HasMaxLength(255);



                entity.HasOne(x => x.Supplier)

                    .WithMany()

                    .HasForeignKey(x => x.SupplierId)

                    .OnDelete(DeleteBehavior.Restrict);

            });



            modelBuilder.Entity<ProductPurchaseEntryItemEntity>(entity =>

            {

                entity.ToTable("ProductPurchaseEntryItems");

                entity.HasKey(x => x.Id);



                entity.Property(x => x.Id).ValueGeneratedOnAdd();

                entity.Property(x => x.ProductPurchaseEntryId).IsRequired();

                entity.Property(x => x.ProductId).IsRequired();

                entity.Property(x => x.Quantity).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.UnitCost).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.MarkupPercent).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.CalculatedUnitPrice).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.TotalCost).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.CreatedAt).IsRequired();

                entity.Property(x => x.CreatedBy).HasMaxLength(255).IsRequired();

                entity.Property(x => x.UpdatedAt);

                entity.Property(x => x.UpdatedBy).HasMaxLength(255);



                entity.HasOne(x => x.ProductPurchaseEntry)

                    .WithMany(x => x.Items)

                    .HasForeignKey(x => x.ProductPurchaseEntryId)

                    .OnDelete(DeleteBehavior.Cascade);



                entity.HasOne(x => x.Product)

                    .WithMany()

                    .HasForeignKey(x => x.ProductId)

                    .OnDelete(DeleteBehavior.Restrict);

            });



            modelBuilder.Entity<StockAdjustmentEntity>(entity =>

            {

                entity.ToTable("StockAdjustments");

                entity.HasKey(x => x.Id);



                entity.Property(x => x.Id).ValueGeneratedOnAdd();

                entity.Property(x => x.Number).HasMaxLength(20).IsRequired();

                entity.Property(x => x.AdjustmentDate).IsRequired();

                entity.Property(x => x.Reason).HasMaxLength(255).IsRequired();

                entity.Property(x => x.Notes).HasMaxLength(1000);

                entity.Property(x => x.IsConfirmed).IsRequired();

                entity.Property(x => x.CreatedAt).IsRequired();

                entity.Property(x => x.CreatedBy).HasMaxLength(255).IsRequired();

                entity.Property(x => x.UpdatedAt);

                entity.Property(x => x.UpdatedBy).HasMaxLength(255);

            });



            modelBuilder.Entity<StockAdjustmentItemEntity>(entity =>

            {

                entity.ToTable("StockAdjustmentItems");

                entity.HasKey(x => x.Id);



                entity.Property(x => x.Id).ValueGeneratedOnAdd();

                entity.Property(x => x.StockAdjustmentId).IsRequired();

                entity.Property(x => x.ProductId).IsRequired();

                entity.Property(x => x.Quantity).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.Notes).HasMaxLength(1000);

                entity.Property(x => x.CreatedAt).IsRequired();

                entity.Property(x => x.CreatedBy).HasMaxLength(255).IsRequired();

                entity.Property(x => x.UpdatedAt);

                entity.Property(x => x.UpdatedBy).HasMaxLength(255);



                entity.HasOne(x => x.StockAdjustment)

                    .WithMany(x => x.Items)

                    .HasForeignKey(x => x.StockAdjustmentId)

                    .OnDelete(DeleteBehavior.Cascade);



                entity.HasOne(x => x.Product)

                    .WithMany()

                    .HasForeignKey(x => x.ProductId)

                    .OnDelete(DeleteBehavior.Restrict);

            });



            modelBuilder.Entity<StockMovementEntity>(entity =>

            {

                entity.ToTable("StockMovements");

                entity.HasKey(x => x.Id);



                entity.Property(x => x.Id).ValueGeneratedOnAdd();

                entity.Property(x => x.ProductId).IsRequired();

                entity.Property(x => x.MovementType).IsRequired();

                entity.Property(x => x.Quantity).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.BalanceAfter).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.ProductPurchaseEntryItemId);

                entity.Property(x => x.StockAdjustmentItemId);

                entity.Property(x => x.SaleId);

                entity.Property(x => x.OriginDocumentNumber).HasMaxLength(20);

                entity.Property(x => x.Notes).HasMaxLength(1000);

                entity.Property(x => x.CreatedAt).IsRequired();

                entity.Property(x => x.CreatedBy).HasMaxLength(255).IsRequired();

                entity.Property(x => x.UpdatedAt);

                entity.Property(x => x.UpdatedBy).HasMaxLength(255);



                entity.HasOne(x => x.Product)

                    .WithMany()

                    .HasForeignKey(x => x.ProductId)

                    .OnDelete(DeleteBehavior.Restrict);

            });



            modelBuilder.Entity<ProposalEntity>(entity =>

            {

                entity.ToTable("Proposals");

                entity.HasKey(x => x.Id);



                entity.Property(x => x.Id).ValueGeneratedOnAdd();

                entity.Property(x => x.Number).HasMaxLength(20).IsRequired();

                entity.Property(x => x.ExternalClientId).HasMaxLength(255).IsRequired();

                entity.Property(x => x.SellerEmail).HasMaxLength(255).IsRequired();

                entity.Property(x => x.PaymentConditionId).IsRequired();

                entity.Property(x => x.SaleType).IsRequired();

                entity.Property(x => x.Status).IsRequired();

                entity.Property(x => x.ValidUntil);

                entity.Property(x => x.SubtotalAmount).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.DiscountAmount).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.TaxAmount).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.TotalAmount).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.HasStockWarning).IsRequired();

                entity.Property(x => x.Notes).HasMaxLength(1000);

                entity.Property(x => x.CreatedAt).IsRequired();

                entity.Property(x => x.CreatedBy).HasMaxLength(255).IsRequired();

                entity.Property(x => x.UpdatedAt);

                entity.Property(x => x.UpdatedBy).HasMaxLength(255);



                entity.HasOne(x => x.PaymentCondition)

                    .WithMany()

                    .HasForeignKey(x => x.PaymentConditionId)

                    .OnDelete(DeleteBehavior.Restrict);

            });



            modelBuilder.Entity<ProposalItemEntity>(entity =>

            {

                entity.ToTable("ProposalItems");

                entity.HasKey(x => x.Id);



                entity.Property(x => x.Id).ValueGeneratedOnAdd();

                entity.Property(x => x.ProposalId).IsRequired();

                entity.Property(x => x.ProductId).IsRequired();

                entity.Property(x => x.Quantity).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.UnitPrice).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.DiscountPercent).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.DiscountAmount).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.LineSubtotal).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.IcmsPercent).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.IcmsAmount).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.IssPercent).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.IssAmount).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.PisPercent).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.PisAmount).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.CofinsPercent).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.CofinsAmount).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.LineTotal).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.StockUnavailable).IsRequired();

                entity.Property(x => x.CreatedAt).IsRequired();

                entity.Property(x => x.CreatedBy).HasMaxLength(255).IsRequired();

                entity.Property(x => x.UpdatedAt);

                entity.Property(x => x.UpdatedBy).HasMaxLength(255);



                entity.HasOne(x => x.Proposal)

                    .WithMany(x => x.Items)

                    .HasForeignKey(x => x.ProposalId)

                    .OnDelete(DeleteBehavior.Cascade);



                entity.HasOne(x => x.Product)

                    .WithMany()

                    .HasForeignKey(x => x.ProductId)

                    .OnDelete(DeleteBehavior.Restrict);

            });



            modelBuilder.Entity<SaleEntity>(entity =>

            {

                entity.ToTable("Sales");

                entity.HasKey(x => x.Id);



                entity.Property(x => x.Id).ValueGeneratedOnAdd();

                entity.Property(x => x.Number).HasMaxLength(20).IsRequired();

                entity.Property(x => x.ProposalId);

                entity.Property(x => x.ExternalClientId).HasMaxLength(255).IsRequired();

                entity.Property(x => x.ExternalBillingId).HasMaxLength(255);

                entity.Property(x => x.SellerEmail).HasMaxLength(255).IsRequired();

                entity.Property(x => x.PaymentConditionId).IsRequired();

                entity.Property(x => x.SaleType).IsRequired();

                entity.Property(x => x.Status).IsRequired();

                entity.Property(x => x.SoldAt);

                entity.Property(x => x.SubtotalAmount).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.DiscountAmount).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.TaxAmount).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.CommissionAmount).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.TotalAmount).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.CreatedAt).IsRequired();

                entity.Property(x => x.CreatedBy).HasMaxLength(255).IsRequired();

                entity.Property(x => x.UpdatedAt);

                entity.Property(x => x.UpdatedBy).HasMaxLength(255);



                entity.HasOne(x => x.Proposal)

                    .WithMany()

                    .HasForeignKey(x => x.ProposalId)

                    .OnDelete(DeleteBehavior.SetNull);



                entity.HasOne(x => x.PaymentCondition)

                    .WithMany()

                    .HasForeignKey(x => x.PaymentConditionId)

                    .OnDelete(DeleteBehavior.Restrict);

            });



            modelBuilder.Entity<SaleItemEntity>(entity =>

            {

                entity.ToTable("SaleItems");

                entity.HasKey(x => x.Id);



                entity.Property(x => x.Id).ValueGeneratedOnAdd();

                entity.Property(x => x.SaleId).IsRequired();

                entity.Property(x => x.ProductId).IsRequired();

                entity.Property(x => x.Quantity).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.UnitPrice).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.DiscountPercent).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.DiscountAmount).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.LineSubtotal).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.IcmsPercent).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.IcmsAmount).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.IssPercent).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.IssAmount).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.PisPercent).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.PisAmount).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.CofinsPercent).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.CofinsAmount).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.LineTotal).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.CreatedAt).IsRequired();

                entity.Property(x => x.CreatedBy).HasMaxLength(255).IsRequired();

                entity.Property(x => x.UpdatedAt);

                entity.Property(x => x.UpdatedBy).HasMaxLength(255);



                entity.HasOne(x => x.Sale)

                    .WithMany(x => x.Items)

                    .HasForeignKey(x => x.SaleId)

                    .OnDelete(DeleteBehavior.Cascade);



                entity.HasOne(x => x.Product)

                    .WithMany()

                    .HasForeignKey(x => x.ProductId)

                    .OnDelete(DeleteBehavior.Restrict);

            });



            modelBuilder.Entity<DiscountEntity>(entity =>

            {

                entity.ToTable("Discounts");

                entity.HasKey(x => x.Id);



                entity.Property(x => x.Id).ValueGeneratedOnAdd();

                entity.Property(x => x.Scope).IsRequired();

                entity.Property(x => x.ProposalId);

                entity.Property(x => x.SaleId);

                entity.Property(x => x.ProposalItemId);

                entity.Property(x => x.SaleItemId);

                entity.Property(x => x.DiscountPercent).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.DiscountAmount).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.AppliedAmount).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.AuthorizedBy).HasMaxLength(255).IsRequired();

                entity.Property(x => x.AuthorizedRole).HasMaxLength(255).IsRequired();

                entity.Property(x => x.CreatedAt).IsRequired();

                entity.Property(x => x.CreatedBy).HasMaxLength(255).IsRequired();

                entity.Property(x => x.UpdatedAt);

                entity.Property(x => x.UpdatedBy).HasMaxLength(255);

            });



            modelBuilder.Entity<CommissionEntity>(entity =>

            {

                entity.ToTable("Commissions");

                entity.HasKey(x => x.Id);



                entity.Property(x => x.Id).ValueGeneratedOnAdd();

                entity.Property(x => x.SaleId).IsRequired();

                entity.Property(x => x.SaleItemId);

                entity.Property(x => x.CalculationScope).IsRequired();

                entity.Property(x => x.SellerEmail).HasMaxLength(255).IsRequired();

                entity.Property(x => x.CommissionPercent).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.BaseAmount).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.CommissionAmount).HasPrecision(18, 4).IsRequired();

                entity.Property(x => x.Status).IsRequired();

                entity.Property(x => x.CreatedAt).IsRequired();

                entity.Property(x => x.CreatedBy).HasMaxLength(255).IsRequired();

                entity.Property(x => x.UpdatedAt);

                entity.Property(x => x.UpdatedBy).HasMaxLength(255);



                entity.HasOne(x => x.Sale)

                    .WithMany(x => x.Commissions)

                    .HasForeignKey(x => x.SaleId)

                    .OnDelete(DeleteBehavior.Cascade);



                entity.HasOne(x => x.SaleItem)

                    .WithMany()

                    .HasForeignKey(x => x.SaleItemId)

                    .OnDelete(DeleteBehavior.SetNull);

            });

        }



        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)

        {

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        }



        #region DbSets



        public DbSet<TestEntity> Tests { get; set; }

        public DbSet<SupplierEntity> Suppliers { get; set; }

        public DbSet<UnitOfMeasureEntity> UnitOfMeasures { get; set; }

        public DbSet<ProductEntity> Products { get; set; }

        public DbSet<ProductStockEntity> ProductStocks { get; set; }

        public DbSet<PaymentConditionEntity> PaymentConditions { get; set; }

        public DbSet<DiscountRuleEntity> DiscountRules { get; set; }

        public DbSet<CommissionRuleEntity> CommissionRules { get; set; }

        public DbSet<DocumentSequenceEntity> DocumentSequences { get; set; }

        public DbSet<ProductPurchaseEntryEntity> ProductPurchaseEntries { get; set; }

        public DbSet<ProductPurchaseEntryItemEntity> ProductPurchaseEntryItems { get; set; }

        public DbSet<StockAdjustmentEntity> StockAdjustments { get; set; }

        public DbSet<StockAdjustmentItemEntity> StockAdjustmentItems { get; set; }

        public DbSet<StockMovementEntity> StockMovements { get; set; }

        public DbSet<ProposalEntity> Proposals { get; set; }

        public DbSet<ProposalItemEntity> ProposalItems { get; set; }

        public DbSet<SaleEntity> Sales { get; set; }

        public DbSet<SaleItemEntity> SaleItems { get; set; }

        public DbSet<DiscountEntity> Discounts { get; set; }

        public DbSet<CommissionEntity> Commissions { get; set; }



        #endregion

    }

}


