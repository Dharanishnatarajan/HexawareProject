using Microsoft.EntityFrameworkCore;
using Application.Models;

namespace Application.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Expense>()
                .Property(e => e.value)
                .HasPrecision(18, 2); // 18 digits total, 2 after decimal

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Expense> Expenses { get; set; }
    }
}
