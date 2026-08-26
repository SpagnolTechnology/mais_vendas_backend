using FluentMigrator;

namespace Migrations.Migrations._2026._06
{
    [Migration(202606261415)]
    public class Mig_202606261415_Tests : Migration
    {
        public override void Up()
        {
            if (!Schema.Table("Tests").Exists())
            {
                Create.Table("Tests")
                    .WithColumn("Id").AsInt32().PrimaryKey("PK_Tests").Identity()
                    .WithColumn("Name").AsAnsiString(255).NotNullable()
                    .WithColumn("Description").AsAnsiString(1000).Nullable()
                    .WithColumn("IsActive").AsBoolean().NotNullable()
                    .WithColumn("ReferenceDate").AsDateTime().NotNullable()
                    .WithColumn("CreatedAt").AsDateTime().NotNullable()
                    .WithColumn("CreatedBy").AsAnsiString(255).NotNullable()
                    .WithColumn("UpdatedAt").AsDateTime().Nullable()
                    .WithColumn("UpdatedBy").AsAnsiString(255).Nullable();
            }
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}
