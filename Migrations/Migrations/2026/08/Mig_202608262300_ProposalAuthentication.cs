using FluentMigrator;

namespace Migrations.Migrations._2026._08
{
    [Migration(202608262300)]
    public class Mig_202608262300_ProposalAuthentication : Migration
    {
        public override void Up()
        {
            if (!Schema.Table("ProposalAuthentication").Exists())
            {
                Create.Table("ProposalAuthentication")
                    .WithColumn("ProposalUuid").AsAnsiString(36).PrimaryKey("PK_ProposalAuthentication")
                    .WithColumn("CompanyCnpj").AsAnsiString(14).NotNullable();
            }
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}
