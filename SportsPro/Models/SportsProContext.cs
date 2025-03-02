using Microsoft.EntityFrameworkCore;

namespace SportsPro.Models
{
    public class SportsProContext : DbContext
    {
        public SportsProContext(DbContextOptions<SportsProContext> options)
            : base(options)
        { }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Technician> Technicians { get; set; } = null!;
        public DbSet<Country> Countries { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Incident> Incidents { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }
    }
}