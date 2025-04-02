using Microsoft.EntityFrameworkCore;

namespace BlogAPI_EFCore.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Blog> Blogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Blog>()
                .ToTable("blogs")
                .HasKey(e => e.Id);
        }

        public async Task DistributeTableAsync()
        {
            await Database.ExecuteSqlRawAsync("SELECT create_distributed_table('blogs', 'id')");
        }
    }
}
