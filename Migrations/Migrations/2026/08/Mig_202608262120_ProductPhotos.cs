using FluentMigrator;

namespace Migrations.Migrations._2026._08
{
    [Migration(202608262120)]
    public class Mig_202608262120_ProductPhotos : Migration
    {
        public override void Up()
        {
            if (!Schema.Table("ProductPhotos").Exists())
            {
                Create.Table("ProductPhotos")
                    .WithColumn("Id").AsInt32().PrimaryKey("PK_ProductPhotos").Identity()
                    .WithColumn("ProductId").AsInt32().NotNullable()
                    .WithColumn("ImageUrl").AsAnsiString(2048).NotNullable()
                    .WithColumn("CreatedAt").AsDateTime().NotNullable()
                    .WithColumn("CreatedBy").AsAnsiString(255).NotNullable()
                    .WithColumn("UpdatedAt").AsDateTime().Nullable()
                    .WithColumn("UpdatedBy").AsAnsiString(255).Nullable();

                Create.ForeignKey("FK_ProductPhotos_Products")
                    .FromTable("ProductPhotos").ForeignColumn("ProductId")
                    .ToTable("Products").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.Cascade);

                Create.Index("IX_ProductPhotos_ProductId")
                    .OnTable("ProductPhotos")
                    .OnColumn("ProductId").Ascending();
            }
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}
