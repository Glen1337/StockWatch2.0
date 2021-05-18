using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DegenApp.Models;
using DegenApp.Enums;

namespace DegenApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seeds


            // Portfolios

            // User 1's ports
            modelBuilder.Entity<Portfolio>().HasData(new Portfolio 
            {
                UserId = "1",
                PortfolioId = 1L,
                Title = "User1's portfolio",
                //TotalMarketValue = 1000.01M,
                GainLoss = 768.00M,
                Type = "Investing",
                //CreationDate = new DateTime(2020, 12, 19, 16, 19, 35, 522, DateTimeKind.Local).AddTicks(6707)
            });
            modelBuilder.Entity<Portfolio>().HasData(new Portfolio
            {
                UserId = "1",
                PortfolioId = 2L,
                Title = "User1's Roth IRA Portfolio",
                TotalMarketValue = 5204.99M,
                GainLoss = -324.67M,
                Type = "Roth IRA",
                //CreationDate = new DateTime(2020, 12, 19, 16, 19, 35, 522, DateTimeKind.Local).AddTicks(3249)
            });

            // User 2's port
            modelBuilder.Entity<Portfolio>().HasData(new Portfolio
            {
                UserId = "2",
                PortfolioId = 3L,
                Title = "User2's Primary Portfolio",
                TotalMarketValue = 52064.29M,
                GainLoss = 19874.73M,
                Type = "Speculation",
                //CreationDate = new DateTime(2020, 12, 19, 16, 19, 35, 522, DateTimeKind.Local).AddTicks(2673)
            });

            // Holdings

            // Port 1's holdings (user 1)
            modelBuilder.Entity<Holding>().HasData(new Holding
            { 
                //Action = Action,
                OrderType = OrderType.Buy,
                CostBasis = 20.22M,
                Quantity = 1000,
                Symbol = "NKE" ,
                PortfolioId = 1,
                HoldingId = 1,
                //Type = "share",
                SecurityType = SecurityType.Share,
                ReinvestDivs = true
            });
            modelBuilder.Entity<Holding>().HasData(new Holding
            {
                //Action = "buy",
                OrderType = OrderType.Buy,
                CostBasis = 17.98M,
                Quantity = 3000,
                Symbol = "SNAP",
                PortfolioId = 1,
                HoldingId = 2,
                //Type = "share",
                SecurityType = SecurityType.Share,
                ReinvestDivs = false
            });
            
            // Port 2's holding (user 1)
            modelBuilder.Entity<Holding>().HasData(new Holding
            {
                //Action = "buy",
                OrderType = OrderType.Buy,
                CostBasis = 20.22M,
                Quantity = 70,
                Symbol = "FSLR",
                PortfolioId = 2,
                HoldingId = 3,
                //Type = "share",
                SecurityType = SecurityType.Share,
                ReinvestDivs = false
            });
            
            // Port 3's holding (user 2)
            modelBuilder.Entity<Holding>().HasData(new Holding
            {
                //Action = "buy",
                OrderType = OrderType.Buy,
                CostBasis = 160.22M,
                Quantity = 800,
                Symbol = "SPOT",
                PortfolioId = 3,
                HoldingId = 4,
                //Type = "share",
                SecurityType = SecurityType.Share,
                ReinvestDivs = true
            });
            modelBuilder.Entity<Holding>().HasData(new Holding
            { 
                //Action = "shortsell",
                OrderType = OrderType.Buy,
                CostBasis = 85.09M, 
                Quantity = 500,
                Symbol = "ROKU",
                PortfolioId = 3,
                HoldingId = 5,
                //Type = "share",
                SecurityType = SecurityType.Share,
                ReinvestDivs = false
            });

            //  Store enums in db as strings instead of ints

            modelBuilder
                .Entity<Holding>()
                .Property(h => h.OrderType)
                .HasConversion(
                    o => o.ToString(),
                    o => (OrderType)Enum.Parse(typeof(OrderType), o));

            modelBuilder
                .Entity<Holding>()
                .Property(h => h.SecurityType)
                .HasConversion(
                    s => s.ToString(),
                    s => (SecurityType)Enum.Parse(typeof(SecurityType), s));

            modelBuilder
                .Entity<WatchItem>()
                .Property(w => w.Outlook)
                .HasConversion(
                    o => o.ToString(),
                    o => (Outlook)Enum.Parse(typeof(Outlook), o));
        }

        public DbSet<Portfolio> Portfolios { get; set; }

        public DbSet<Holding> Holdings { get; set; }

        public DbSet<WatchItem> WatchItems { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<User> Users { get; set; }

    }
}
