using Microsoft.EntityFrameworkCore;

namespace PingSite
{
    public class ApplicationContext: DbContext
    {
        public DbSet<Site> urlsite { get; set; }

        public DbSet<Interval> intervalsite { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=127.0.0.1;User Id=postgres;Password=nagimullin;Database=Site;");
        }
    }
}
