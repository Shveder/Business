using Business_monitoring.Models;
using Microsoft.EntityFrameworkCore;

namespace Business_monitoring.Data
{
    public class Context : DbContext
    {
        
        public DbSet<UserModel> Users { get; set; }
        public DbSet<Business> Business { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Deposits> Deposits { get; set; }
        
        public DbSet<Expert> Expert { get; set; }
        public DbSet<ExpertView> ExpertView { get; set; }
        public DbSet<GainsOfCompany> GainsOfCompany { get; set; }
        public DbSet<LoginHistory> LoginHistory { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        
        public DbSet<Owners> Owners { get; set; }
        public DbSet<PricesOfShares> PricesOfShares { get; set; }
        public DbSet<PurchaceOfView> PurchacesOfView { get; set; }
        public DbSet<RecentPasswords> RecentPasswords { get; set; }
        public DbSet<RecentPricesOfBusiness> RecentPricesOfBusiness { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        
        public DbSet<Offer> Offers { get; set; }

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