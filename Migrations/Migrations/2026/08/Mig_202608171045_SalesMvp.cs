using FluentMigrator;



namespace Migrations.Migrations._2026._08

{

    [Migration(202608171045)]

    public class Mig_202608171045_SalesMvp : Migration

    {

        public override void Up()

        {

            if (!Schema.Table("Suppliers").Exists())

            {

                Create.Table("Suppliers")

                    .WithColumn("Id").AsInt32().PrimaryKey("PK_Suppliers").Identity()

                    .WithColumn("Name").AsAnsiString(255).NotNullable()

                    .WithColumn("Document").AsAnsiString(20).NotNullable()

                    .WithColumn("Email").AsAnsiString(255).Nullable()

                    .WithColumn("Phone").AsAnsiString(50).Nullable()

                    .WithColumn("Address").AsAnsiString(500).Nullable()

                    .WithColumn("City").AsAnsiString(100).Nullable()

                    .WithColumn("State").AsAnsiString(50).Nullable()

                    .WithColumn("ZipCode").AsAnsiString(20).Nullable()

                    .WithColumn("IsActive").AsBoolean().NotNullable()

                    .WithColumn("CreatedAt").AsDateTime().NotNullable()

                    .WithColumn("CreatedBy").AsAnsiString(255).NotNullable()

                    .WithColumn("UpdatedAt").AsDateTime().Nullable()

                    .WithColumn("UpdatedBy").AsAnsiString(255).Nullable();

            }



            if (!Schema.Table("UnitOfMeasures").Exists())

            {

                Create.Table("UnitOfMeasures")

                    .WithColumn("Id").AsInt32().PrimaryKey("PK_UnitOfMeasures").Identity()

                    .WithColumn("Name").AsAnsiString(255).NotNullable()

                    .WithColumn("Abbreviation").AsAnsiString(20).NotNullable()

                    .WithColumn("IsActive").AsBoolean().NotNullable()

                    .WithColumn("CreatedAt").AsDateTime().NotNullable()

                    .WithColumn("CreatedBy").AsAnsiString(255).NotNullable()

                    .WithColumn("UpdatedAt").AsDateTime().Nullable()

                    .WithColumn("UpdatedBy").AsAnsiString(255).Nullable();

            }



            if (!Schema.Table("Products").Exists())

            {

                Create.Table("Products")

                    .WithColumn("Id").AsInt32().PrimaryKey("PK_Products").Identity()

                    .WithColumn("UnitOfMeasureId").AsInt32().NotNullable()

                    .WithColumn("Name").AsAnsiString(255).NotNullable()

                    .WithColumn("MaskName").AsAnsiString(255).Nullable()

                    .WithColumn("Sku").AsAnsiString(100).NotNullable()

                    .WithColumn("Ean").AsAnsiString(14).Nullable()

                    .WithColumn("Description").AsAnsiString(1000).Nullable()

                    .WithColumn("UnitPrice").AsDecimal(18, 4).NotNullable()

                    .WithColumn("CostPrice").AsDecimal(18, 4).NotNullable()

                    .WithColumn("MarkupPercent").AsDecimal(18, 4).NotNullable()

                    .WithColumn("IcmsPercent").AsDecimal(18, 4).NotNullable()

                    .WithColumn("IssPercent").AsDecimal(18, 4).NotNullable()

                    .WithColumn("PisPercent").AsDecimal(18, 4).NotNullable()

                    .WithColumn("CofinsPercent").AsDecimal(18, 4).NotNullable()

                    .WithColumn("IsActive").AsBoolean().NotNullable()

                    .WithColumn("CreatedAt").AsDateTime().NotNullable()

                    .WithColumn("CreatedBy").AsAnsiString(255).NotNullable()

                    .WithColumn("UpdatedAt").AsDateTime().Nullable()

                    .WithColumn("UpdatedBy").AsAnsiString(255).Nullable();



                Create.ForeignKey("FK_Products_UnitOfMeasures")

                    .FromTable("Products").ForeignColumn("UnitOfMeasureId")

                    .ToTable("UnitOfMeasures").PrimaryColumn("Id");



                Execute.Sql(
                    "CREATE UNIQUE INDEX IF NOT EXISTS \"IX_Products_Ean\" ON \"Products\" (\"Ean\") WHERE \"Ean\" IS NOT NULL");

            }



            if (!Schema.Table("ProductStocks").Exists())

            {

                Create.Table("ProductStocks")

                    .WithColumn("Id").AsInt32().PrimaryKey("PK_ProductStocks").Identity()

                    .WithColumn("ProductId").AsInt32().NotNullable()

                    .WithColumn("Quantity").AsDecimal(18, 4).NotNullable()

                    .WithColumn("MinimumQuantity").AsDecimal(18, 4).Nullable()

                    .WithColumn("CreatedAt").AsDateTime().NotNullable()

                    .WithColumn("CreatedBy").AsAnsiString(255).NotNullable()

                    .WithColumn("UpdatedAt").AsDateTime().Nullable()

                    .WithColumn("UpdatedBy").AsAnsiString(255).Nullable();



                Create.ForeignKey("FK_ProductStocks_Products")

                    .FromTable("ProductStocks").ForeignColumn("ProductId")

                    .ToTable("Products").PrimaryColumn("Id")

                    .OnDelete(System.Data.Rule.Cascade);



                Create.Index("IX_ProductStocks_ProductId")

                    .OnTable("ProductStocks")

                    .OnColumn("ProductId").Ascending()

                    .WithOptions().Unique();

            }



            if (!Schema.Table("PaymentConditions").Exists())

            {

                Create.Table("PaymentConditions")

                    .WithColumn("Id").AsInt32().PrimaryKey("PK_PaymentConditions").Identity()

                    .WithColumn("Name").AsAnsiString(255).NotNullable()

                    .WithColumn("InstallmentCount").AsInt32().NotNullable()

                    .WithColumn("DaysUntilFirstDue").AsInt32().NotNullable()

                    .WithColumn("DaysBetweenInstallments").AsInt32().NotNullable()

                    .WithColumn("CashDiscountPercent").AsDecimal(18, 4).NotNullable()

                    .WithColumn("IsActive").AsBoolean().NotNullable()

                    .WithColumn("CreatedAt").AsDateTime().NotNullable()

                    .WithColumn("CreatedBy").AsAnsiString(255).NotNullable()

                    .WithColumn("UpdatedAt").AsDateTime().Nullable()

                    .WithColumn("UpdatedBy").AsAnsiString(255).Nullable();

            }



            if (!Schema.Table("DiscountRules").Exists())

            {

                Create.Table("DiscountRules")

                    .WithColumn("Id").AsInt32().PrimaryKey("PK_DiscountRules").Identity()

                    .WithColumn("Role").AsAnsiString(255).Nullable()

                    .WithColumn("UserEmail").AsAnsiString(255).Nullable()

                    .WithColumn("MaxDiscountPercent").AsDecimal(18, 4).NotNullable()

                    .WithColumn("MaxDiscountAmount").AsDecimal(18, 4).Nullable()

                    .WithColumn("IsActive").AsBoolean().NotNullable()

                    .WithColumn("CreatedAt").AsDateTime().NotNullable()

                    .WithColumn("CreatedBy").AsAnsiString(255).NotNullable()

                    .WithColumn("UpdatedAt").AsDateTime().Nullable()

                    .WithColumn("UpdatedBy").AsAnsiString(255).Nullable();

            }



            if (!Schema.Table("CommissionRules").Exists())

            {

                Create.Table("CommissionRules")

                    .WithColumn("Id").AsInt32().PrimaryKey("PK_CommissionRules").Identity()

                    .WithColumn("ProductId").AsInt32().Nullable()

                    .WithColumn("CalculationScope").AsInt32().Nullable()

                    .WithColumn("CommissionPercent").AsDecimal(18, 4).NotNullable()

                    .WithColumn("IsActive").AsBoolean().NotNullable()

                    .WithColumn("CreatedAt").AsDateTime().NotNullable()

                    .WithColumn("CreatedBy").AsAnsiString(255).NotNullable()

                    .WithColumn("UpdatedAt").AsDateTime().Nullable()

                    .WithColumn("UpdatedBy").AsAnsiString(255).Nullable();



                Create.ForeignKey("FK_CommissionRules_Products")

                    .FromTable("CommissionRules").ForeignColumn("ProductId")

                    .ToTable("Products").PrimaryColumn("Id")

                    .OnDelete(System.Data.Rule.SetNull);

            }



            if (!Schema.Table("DocumentSequences").Exists())

            {

                Create.Table("DocumentSequences")

                    .WithColumn("Id").AsInt32().PrimaryKey("PK_DocumentSequences").Identity()

                    .WithColumn("DocumentType").AsInt32().NotNullable()

                    .WithColumn("Year").AsInt32().NotNullable()

                    .WithColumn("LastNumber").AsInt32().NotNullable()

                    .WithColumn("CreatedAt").AsDateTime().NotNullable()

                    .WithColumn("CreatedBy").AsAnsiString(255).NotNullable()

                    .WithColumn("UpdatedAt").AsDateTime().Nullable()

                    .WithColumn("UpdatedBy").AsAnsiString(255).Nullable();



                Create.Index("IX_DocumentSequences_DocumentType_Year")

                    .OnTable("DocumentSequences")

                    .OnColumn("DocumentType").Ascending()

                    .OnColumn("Year").Ascending()

                    .WithOptions().Unique();

            }



            if (!Schema.Table("ProductPurchaseEntries").Exists())

            {

                Create.Table("ProductPurchaseEntries")

                    .WithColumn("Id").AsInt32().PrimaryKey("PK_ProductPurchaseEntries").Identity()

                    .WithColumn("Number").AsAnsiString(20).NotNullable()

                    .WithColumn("SupplierId").AsInt32().NotNullable()

                    .WithColumn("InvoiceNumber").AsAnsiString(50).NotNullable()

                    .WithColumn("InvoiceSeries").AsAnsiString(10).NotNullable()

                    .WithColumn("InvoiceKey").AsAnsiString(44).Nullable()

                    .WithColumn("Status").AsInt32().NotNullable()

                    .WithColumn("EntryDate").AsDateTime().NotNullable()

                    .WithColumn("Notes").AsAnsiString(1000).Nullable()

                    .WithColumn("CreatedAt").AsDateTime().NotNullable()

                    .WithColumn("CreatedBy").AsAnsiString(255).NotNullable()

                    .WithColumn("UpdatedAt").AsDateTime().Nullable()

                    .WithColumn("UpdatedBy").AsAnsiString(255).Nullable();



                Create.ForeignKey("FK_ProductPurchaseEntries_Suppliers")

                    .FromTable("ProductPurchaseEntries").ForeignColumn("SupplierId")

                    .ToTable("Suppliers").PrimaryColumn("Id");

            }



            if (!Schema.Table("ProductPurchaseEntryItems").Exists())

            {

                Create.Table("ProductPurchaseEntryItems")

                    .WithColumn("Id").AsInt32().PrimaryKey("PK_ProductPurchaseEntryItems").Identity()

                    .WithColumn("ProductPurchaseEntryId").AsInt32().NotNullable()

                    .WithColumn("ProductId").AsInt32().NotNullable()

                    .WithColumn("Quantity").AsDecimal(18, 4).NotNullable()

                    .WithColumn("UnitCost").AsDecimal(18, 4).NotNullable()

                    .WithColumn("MarkupPercent").AsDecimal(18, 4).NotNullable()

                    .WithColumn("CalculatedUnitPrice").AsDecimal(18, 4).NotNullable()

                    .WithColumn("TotalCost").AsDecimal(18, 4).NotNullable()

                    .WithColumn("CreatedAt").AsDateTime().NotNullable()

                    .WithColumn("CreatedBy").AsAnsiString(255).NotNullable()

                    .WithColumn("UpdatedAt").AsDateTime().Nullable()

                    .WithColumn("UpdatedBy").AsAnsiString(255).Nullable();



                Create.ForeignKey("FK_ProductPurchaseEntryItems_ProductPurchaseEntries")

                    .FromTable("ProductPurchaseEntryItems").ForeignColumn("ProductPurchaseEntryId")

                    .ToTable("ProductPurchaseEntries").PrimaryColumn("Id")

                    .OnDelete(System.Data.Rule.Cascade);



                Create.ForeignKey("FK_ProductPurchaseEntryItems_Products")

                    .FromTable("ProductPurchaseEntryItems").ForeignColumn("ProductId")

                    .ToTable("Products").PrimaryColumn("Id");

            }



            if (!Schema.Table("StockAdjustments").Exists())

            {

                Create.Table("StockAdjustments")

                    .WithColumn("Id").AsInt32().PrimaryKey("PK_StockAdjustments").Identity()

                    .WithColumn("Number").AsAnsiString(20).NotNullable()

                    .WithColumn("AdjustmentDate").AsDateTime().NotNullable()

                    .WithColumn("Reason").AsAnsiString(255).NotNullable()

                    .WithColumn("Notes").AsAnsiString(1000).Nullable()

                    .WithColumn("IsConfirmed").AsBoolean().NotNullable()

                    .WithColumn("CreatedAt").AsDateTime().NotNullable()

                    .WithColumn("CreatedBy").AsAnsiString(255).NotNullable()

                    .WithColumn("UpdatedAt").AsDateTime().Nullable()

                    .WithColumn("UpdatedBy").AsAnsiString(255).Nullable();

            }



            if (!Schema.Table("StockAdjustmentItems").Exists())

            {

                Create.Table("StockAdjustmentItems")

                    .WithColumn("Id").AsInt32().PrimaryKey("PK_StockAdjustmentItems").Identity()

                    .WithColumn("StockAdjustmentId").AsInt32().NotNullable()

                    .WithColumn("ProductId").AsInt32().NotNullable()

                    .WithColumn("Quantity").AsDecimal(18, 4).NotNullable()

                    .WithColumn("Notes").AsAnsiString(1000).Nullable()

                    .WithColumn("CreatedAt").AsDateTime().NotNullable()

                    .WithColumn("CreatedBy").AsAnsiString(255).NotNullable()

                    .WithColumn("UpdatedAt").AsDateTime().Nullable()

                    .WithColumn("UpdatedBy").AsAnsiString(255).Nullable();



                Create.ForeignKey("FK_StockAdjustmentItems_StockAdjustments")

                    .FromTable("StockAdjustmentItems").ForeignColumn("StockAdjustmentId")

                    .ToTable("StockAdjustments").PrimaryColumn("Id")

                    .OnDelete(System.Data.Rule.Cascade);



                Create.ForeignKey("FK_StockAdjustmentItems_Products")

                    .FromTable("StockAdjustmentItems").ForeignColumn("ProductId")

                    .ToTable("Products").PrimaryColumn("Id");

            }



            if (!Schema.Table("Proposals").Exists())

            {

                Create.Table("Proposals")

                    .WithColumn("Id").AsInt32().PrimaryKey("PK_Proposals").Identity()

                    .WithColumn("ProposalUuid").AsGuid().NotNullable()

                    .WithColumn("Number").AsAnsiString(20).NotNullable()

                    .WithColumn("ExternalClientId").AsAnsiString(255).NotNullable()

                    .WithColumn("SellerEmail").AsAnsiString(255).NotNullable()

                    .WithColumn("PaymentConditionId").AsInt32().NotNullable()

                    .WithColumn("SaleType").AsInt32().NotNullable()

                    .WithColumn("Status").AsInt32().NotNullable()

                    .WithColumn("ValidUntil").AsDateTime().Nullable()

                    .WithColumn("SubtotalAmount").AsDecimal(18, 4).NotNullable()

                    .WithColumn("DiscountAmount").AsDecimal(18, 4).NotNullable()

                    .WithColumn("TaxAmount").AsDecimal(18, 4).NotNullable()

                    .WithColumn("TotalAmount").AsDecimal(18, 4).NotNullable()

                    .WithColumn("HasStockWarning").AsBoolean().NotNullable()

                    .WithColumn("Notes").AsAnsiString(1000).Nullable()

                    .WithColumn("CreatedAt").AsDateTime().NotNullable()

                    .WithColumn("CreatedBy").AsAnsiString(255).NotNullable()

                    .WithColumn("UpdatedAt").AsDateTime().Nullable()

                    .WithColumn("UpdatedBy").AsAnsiString(255).Nullable();



                Create.ForeignKey("FK_Proposals_PaymentConditions")

                    .FromTable("Proposals").ForeignColumn("PaymentConditionId")

                    .ToTable("PaymentConditions").PrimaryColumn("Id");



                Create.Index("IX_Proposals_ProposalUuid")

                    .OnTable("Proposals")

                    .OnColumn("ProposalUuid").Ascending()

                    .WithOptions().Unique();

            }



            if (!Schema.Table("ProposalItems").Exists())

            {

                Create.Table("ProposalItems")

                    .WithColumn("Id").AsInt32().PrimaryKey("PK_ProposalItems").Identity()

                    .WithColumn("ProposalId").AsInt32().NotNullable()

                    .WithColumn("ProductId").AsInt32().NotNullable()

                    .WithColumn("Quantity").AsDecimal(18, 4).NotNullable()

                    .WithColumn("UnitPrice").AsDecimal(18, 4).NotNullable()

                    .WithColumn("DiscountPercent").AsDecimal(18, 4).NotNullable()

                    .WithColumn("DiscountAmount").AsDecimal(18, 4).NotNullable()

                    .WithColumn("LineSubtotal").AsDecimal(18, 4).NotNullable()

                    .WithColumn("IcmsPercent").AsDecimal(18, 4).NotNullable()

                    .WithColumn("IcmsAmount").AsDecimal(18, 4).NotNullable()

                    .WithColumn("IssPercent").AsDecimal(18, 4).NotNullable()

                    .WithColumn("IssAmount").AsDecimal(18, 4).NotNullable()

                    .WithColumn("PisPercent").AsDecimal(18, 4).NotNullable()

                    .WithColumn("PisAmount").AsDecimal(18, 4).NotNullable()

                    .WithColumn("CofinsPercent").AsDecimal(18, 4).NotNullable()

                    .WithColumn("CofinsAmount").AsDecimal(18, 4).NotNullable()

                    .WithColumn("LineTotal").AsDecimal(18, 4).NotNullable()

                    .WithColumn("StockUnavailable").AsBoolean().NotNullable()

                    .WithColumn("CreatedAt").AsDateTime().NotNullable()

                    .WithColumn("CreatedBy").AsAnsiString(255).NotNullable()

                    .WithColumn("UpdatedAt").AsDateTime().Nullable()

                    .WithColumn("UpdatedBy").AsAnsiString(255).Nullable();



                Create.ForeignKey("FK_ProposalItems_Proposals")

                    .FromTable("ProposalItems").ForeignColumn("ProposalId")

                    .ToTable("Proposals").PrimaryColumn("Id")

                    .OnDelete(System.Data.Rule.Cascade);



                Create.ForeignKey("FK_ProposalItems_Products")

                    .FromTable("ProposalItems").ForeignColumn("ProductId")

                    .ToTable("Products").PrimaryColumn("Id");

            }



            if (!Schema.Table("Sales").Exists())

            {

                Create.Table("Sales")

                    .WithColumn("Id").AsInt32().PrimaryKey("PK_Sales").Identity()

                    .WithColumn("Number").AsAnsiString(20).NotNullable()

                    .WithColumn("ProposalId").AsInt32().Nullable()

                    .WithColumn("ExternalClientId").AsAnsiString(255).NotNullable()

                    .WithColumn("ExternalBillingId").AsAnsiString(255).Nullable()

                    .WithColumn("SellerEmail").AsAnsiString(255).NotNullable()

                    .WithColumn("PaymentConditionId").AsInt32().NotNullable()

                    .WithColumn("SaleType").AsInt32().NotNullable()

                    .WithColumn("Status").AsInt32().NotNullable()

                    .WithColumn("SoldAt").AsDateTime().Nullable()

                    .WithColumn("SubtotalAmount").AsDecimal(18, 4).NotNullable()

                    .WithColumn("DiscountAmount").AsDecimal(18, 4).NotNullable()

                    .WithColumn("TaxAmount").AsDecimal(18, 4).NotNullable()

                    .WithColumn("CommissionAmount").AsDecimal(18, 4).NotNullable()

                    .WithColumn("TotalAmount").AsDecimal(18, 4).NotNullable()

                    .WithColumn("CreatedAt").AsDateTime().NotNullable()

                    .WithColumn("CreatedBy").AsAnsiString(255).NotNullable()

                    .WithColumn("UpdatedAt").AsDateTime().Nullable()

                    .WithColumn("UpdatedBy").AsAnsiString(255).Nullable();



                Create.ForeignKey("FK_Sales_Proposals")

                    .FromTable("Sales").ForeignColumn("ProposalId")

                    .ToTable("Proposals").PrimaryColumn("Id")

                    .OnDelete(System.Data.Rule.SetNull);



                Create.ForeignKey("FK_Sales_PaymentConditions")

                    .FromTable("Sales").ForeignColumn("PaymentConditionId")

                    .ToTable("PaymentConditions").PrimaryColumn("Id");

            }



            if (!Schema.Table("SaleItems").Exists())

            {

                Create.Table("SaleItems")

                    .WithColumn("Id").AsInt32().PrimaryKey("PK_SaleItems").Identity()

                    .WithColumn("SaleId").AsInt32().NotNullable()

                    .WithColumn("ProductId").AsInt32().NotNullable()

                    .WithColumn("Quantity").AsDecimal(18, 4).NotNullable()

                    .WithColumn("UnitPrice").AsDecimal(18, 4).NotNullable()

                    .WithColumn("DiscountPercent").AsDecimal(18, 4).NotNullable()

                    .WithColumn("DiscountAmount").AsDecimal(18, 4).NotNullable()

                    .WithColumn("LineSubtotal").AsDecimal(18, 4).NotNullable()

                    .WithColumn("IcmsPercent").AsDecimal(18, 4).NotNullable()

                    .WithColumn("IcmsAmount").AsDecimal(18, 4).NotNullable()

                    .WithColumn("IssPercent").AsDecimal(18, 4).NotNullable()

                    .WithColumn("IssAmount").AsDecimal(18, 4).NotNullable()

                    .WithColumn("PisPercent").AsDecimal(18, 4).NotNullable()

                    .WithColumn("PisAmount").AsDecimal(18, 4).NotNullable()

                    .WithColumn("CofinsPercent").AsDecimal(18, 4).NotNullable()

                    .WithColumn("CofinsAmount").AsDecimal(18, 4).NotNullable()

                    .WithColumn("LineTotal").AsDecimal(18, 4).NotNullable()

                    .WithColumn("CreatedAt").AsDateTime().NotNullable()

                    .WithColumn("CreatedBy").AsAnsiString(255).NotNullable()

                    .WithColumn("UpdatedAt").AsDateTime().Nullable()

                    .WithColumn("UpdatedBy").AsAnsiString(255).Nullable();



                Create.ForeignKey("FK_SaleItems_Sales")

                    .FromTable("SaleItems").ForeignColumn("SaleId")

                    .ToTable("Sales").PrimaryColumn("Id")

                    .OnDelete(System.Data.Rule.Cascade);



                Create.ForeignKey("FK_SaleItems_Products")

                    .FromTable("SaleItems").ForeignColumn("ProductId")

                    .ToTable("Products").PrimaryColumn("Id");

            }



            if (!Schema.Table("StockMovements").Exists())

            {

                Create.Table("StockMovements")

                    .WithColumn("Id").AsInt32().PrimaryKey("PK_StockMovements").Identity()

                    .WithColumn("ProductId").AsInt32().NotNullable()

                    .WithColumn("MovementType").AsInt32().NotNullable()

                    .WithColumn("Quantity").AsDecimal(18, 4).NotNullable()

                    .WithColumn("BalanceAfter").AsDecimal(18, 4).NotNullable()

                    .WithColumn("ProductPurchaseEntryItemId").AsInt32().Nullable()

                    .WithColumn("StockAdjustmentItemId").AsInt32().Nullable()

                    .WithColumn("SaleId").AsInt32().Nullable()

                    .WithColumn("OriginDocumentNumber").AsAnsiString(20).Nullable()

                    .WithColumn("Notes").AsAnsiString(1000).Nullable()

                    .WithColumn("CreatedAt").AsDateTime().NotNullable()

                    .WithColumn("CreatedBy").AsAnsiString(255).NotNullable()

                    .WithColumn("UpdatedAt").AsDateTime().Nullable()

                    .WithColumn("UpdatedBy").AsAnsiString(255).Nullable();



                Create.ForeignKey("FK_StockMovements_Products")

                    .FromTable("StockMovements").ForeignColumn("ProductId")

                    .ToTable("Products").PrimaryColumn("Id");



                Create.ForeignKey("FK_StockMovements_ProductPurchaseEntryItems")

                    .FromTable("StockMovements").ForeignColumn("ProductPurchaseEntryItemId")

                    .ToTable("ProductPurchaseEntryItems").PrimaryColumn("Id")

                    .OnDelete(System.Data.Rule.SetNull);



                Create.ForeignKey("FK_StockMovements_StockAdjustmentItems")

                    .FromTable("StockMovements").ForeignColumn("StockAdjustmentItemId")

                    .ToTable("StockAdjustmentItems").PrimaryColumn("Id")

                    .OnDelete(System.Data.Rule.SetNull);



                Create.ForeignKey("FK_StockMovements_Sales")

                    .FromTable("StockMovements").ForeignColumn("SaleId")

                    .ToTable("Sales").PrimaryColumn("Id")

                    .OnDelete(System.Data.Rule.SetNull);

            }



            if (!Schema.Table("Discounts").Exists())

            {

                Create.Table("Discounts")

                    .WithColumn("Id").AsInt32().PrimaryKey("PK_Discounts").Identity()

                    .WithColumn("Scope").AsInt32().NotNullable()

                    .WithColumn("ProposalId").AsInt32().Nullable()

                    .WithColumn("SaleId").AsInt32().Nullable()

                    .WithColumn("ProposalItemId").AsInt32().Nullable()

                    .WithColumn("SaleItemId").AsInt32().Nullable()

                    .WithColumn("DiscountPercent").AsDecimal(18, 4).NotNullable()

                    .WithColumn("DiscountAmount").AsDecimal(18, 4).NotNullable()

                    .WithColumn("AppliedAmount").AsDecimal(18, 4).NotNullable()

                    .WithColumn("AuthorizedBy").AsAnsiString(255).NotNullable()

                    .WithColumn("AuthorizedRole").AsAnsiString(255).NotNullable()

                    .WithColumn("CreatedAt").AsDateTime().NotNullable()

                    .WithColumn("CreatedBy").AsAnsiString(255).NotNullable()

                    .WithColumn("UpdatedAt").AsDateTime().Nullable()

                    .WithColumn("UpdatedBy").AsAnsiString(255).Nullable();



                Create.ForeignKey("FK_Discounts_Proposals")

                    .FromTable("Discounts").ForeignColumn("ProposalId")

                    .ToTable("Proposals").PrimaryColumn("Id")

                    .OnDelete(System.Data.Rule.SetNull);



                Create.ForeignKey("FK_Discounts_Sales")

                    .FromTable("Discounts").ForeignColumn("SaleId")

                    .ToTable("Sales").PrimaryColumn("Id")

                    .OnDelete(System.Data.Rule.SetNull);



                Create.ForeignKey("FK_Discounts_ProposalItems")

                    .FromTable("Discounts").ForeignColumn("ProposalItemId")

                    .ToTable("ProposalItems").PrimaryColumn("Id")

                    .OnDelete(System.Data.Rule.SetNull);



                Create.ForeignKey("FK_Discounts_SaleItems")

                    .FromTable("Discounts").ForeignColumn("SaleItemId")

                    .ToTable("SaleItems").PrimaryColumn("Id")

                    .OnDelete(System.Data.Rule.SetNull);

            }



            if (!Schema.Table("Commissions").Exists())

            {

                Create.Table("Commissions")

                    .WithColumn("Id").AsInt32().PrimaryKey("PK_Commissions").Identity()

                    .WithColumn("SaleId").AsInt32().NotNullable()

                    .WithColumn("SaleItemId").AsInt32().Nullable()

                    .WithColumn("CalculationScope").AsInt32().NotNullable()

                    .WithColumn("SellerEmail").AsAnsiString(255).NotNullable()

                    .WithColumn("CommissionPercent").AsDecimal(18, 4).NotNullable()

                    .WithColumn("BaseAmount").AsDecimal(18, 4).NotNullable()

                    .WithColumn("CommissionAmount").AsDecimal(18, 4).NotNullable()

                    .WithColumn("Status").AsInt32().NotNullable()

                    .WithColumn("CreatedAt").AsDateTime().NotNullable()

                    .WithColumn("CreatedBy").AsAnsiString(255).NotNullable()

                    .WithColumn("UpdatedAt").AsDateTime().Nullable()

                    .WithColumn("UpdatedBy").AsAnsiString(255).Nullable();



                Create.ForeignKey("FK_Commissions_Sales")

                    .FromTable("Commissions").ForeignColumn("SaleId")

                    .ToTable("Sales").PrimaryColumn("Id")

                    .OnDelete(System.Data.Rule.Cascade);



                Create.ForeignKey("FK_Commissions_SaleItems")

                    .FromTable("Commissions").ForeignColumn("SaleItemId")

                    .ToTable("SaleItems").PrimaryColumn("Id")

                    .OnDelete(System.Data.Rule.SetNull);

            }

        }



        public override void Down()

        {

            throw new NotImplementedException();

        }

    }

}


