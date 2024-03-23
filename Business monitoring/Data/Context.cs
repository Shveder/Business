using Business_monitoring.Models;
using Microsoft.EntityFrameworkCore;

namespace Business_monitoring.Data
{
    public class Context : DbContext
    {
        
        public DbSet<UserModel> Users { get; set; }
        
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                @"Host=localhost;Port=5432;Database=BusinessMonitoring;Username=postgres;Password=postgres"); 
        }
    }
}