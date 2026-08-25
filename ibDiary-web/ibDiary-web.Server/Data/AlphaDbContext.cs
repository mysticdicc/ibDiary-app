using ibDiary_web.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace ibDiary_web.Server.Data
{
    public class AlphaDbContext : DbContext
    {
        public AlphaDbContext(DbContextOptions<AlphaDbContext> options) : base(options)
        {
        }

        public DbSet<AlphaSignupDto> AlphaSignups { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AlphaSignupDto>(entity =>
            {
                entity.ToTable("alpha-signups");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Notes).HasMaxLength(1000);
                entity.Property(e => e.Source).HasMaxLength(100);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });
        }
    }

}
