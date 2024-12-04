using BanhXeoProject.Configuration;
using BanhXeoProject.Entities;
using Microsoft.EntityFrameworkCore;

namespace BanhXeoProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<GroupImage> GroupImages { get; set; }
        public DbSet<ImageDetail> ImageDetails { get; set; }

        public DbSet<User> User { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ImageDetailConfiguration());
        }
    }
}
