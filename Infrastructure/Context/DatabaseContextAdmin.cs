using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class DatabaseContextAdmin : DbContext
    {
        public DatabaseContextAdmin(DbContextOptions<DatabaseContextAdmin> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProposalAuthenticationEntity>(entity =>
            {
                entity.ToTable("ProposalAuthentication");
                entity.HasKey(x => x.ProposalUuid);
                entity.Property(x => x.ProposalUuid).HasMaxLength(36).IsRequired();
                entity.Property(x => x.CompanyCnpj).HasMaxLength(14).IsRequired();
            });
        }

        public DbSet<ProposalAuthenticationEntity> ProposalAuthentication { get; set; }
    }
}
