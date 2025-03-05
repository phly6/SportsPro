using Microsoft.EntityFrameworkCore;

namespace SportsPro.Models
{
    public class SportsProContext : DbContext
    {
        //Create a logger factory for logging SQL queries
        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder =>
            {
                builder.AddConsole(); //Logs to the console 
            });
        public SportsProContext(DbContextOptions<SportsProContext> options)
            : base(options)
        { }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Technician> Technicians { get; set; } = null!;
        public DbSet<Country> Countries { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Incident> Incidents { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                .UseLoggerFactory(MyLoggerFactory) //Log SQL queries to the console
                .EnableSensitiveDataLogging() //Log parameter values
                .EnableDetailedErrors(); //Enable detailed errors
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}