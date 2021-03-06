// <auto-generated />
using System;
using DegenApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DegenApp.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20201227052629_add-seeds")]
    partial class addseeds
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("DegenApp.Models.Holding", b =>
                {
                    b.Property<long>("HoldingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<decimal>("CostBasis")
                        .HasColumnType("Money");

                    b.Property<decimal>("CurrentPrice")
                        .HasColumnType("Money");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsOpen")
                        .HasColumnType("bit");

                    b.Property<string>("OrderType")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<long>("PortfolioId")
                        .HasColumnType("bigint");

                    b.Property<double>("Quantity")
                        .HasColumnType("float");

                    b.Property<bool>("ReinvestDivs")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityType")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<decimal>("StrikePrice")
                        .HasColumnType("Money");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.HasKey("HoldingId");

                    b.HasIndex("PortfolioId");

                    b.ToTable("Holdings");

                    b.HasData(
                        new
                        {
                            HoldingId = 1L,
                            CostBasis = 20.22m,
                            CurrentPrice = 0m,
                            ExpirationDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsOpen = false,
                            OrderType = "Buy",
                            PortfolioId = 1L,
                            Quantity = 1000.0,
                            ReinvestDivs = true,
                            SecurityType = "Share",
                            StrikePrice = 0m,
                            Symbol = "NKE",
                            TransactionDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            HoldingId = 2L,
                            CostBasis = 17.98m,
                            CurrentPrice = 0m,
                            ExpirationDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsOpen = false,
                            OrderType = "Buy",
                            PortfolioId = 1L,
                            Quantity = 3000.0,
                            ReinvestDivs = false,
                            SecurityType = "Share",
                            StrikePrice = 0m,
                            Symbol = "SNAP",
                            TransactionDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            HoldingId = 3L,
                            CostBasis = 20.22m,
                            CurrentPrice = 0m,
                            ExpirationDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsOpen = false,
                            OrderType = "Buy",
                            PortfolioId = 2L,
                            Quantity = 70.0,
                            ReinvestDivs = false,
                            SecurityType = "Share",
                            StrikePrice = 0m,
                            Symbol = "FSLR",
                            TransactionDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            HoldingId = 4L,
                            CostBasis = 160.22m,
                            CurrentPrice = 0m,
                            ExpirationDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsOpen = false,
                            OrderType = "Buy",
                            PortfolioId = 3L,
                            Quantity = 800.0,
                            ReinvestDivs = true,
                            SecurityType = "Share",
                            StrikePrice = 0m,
                            Symbol = "SPOT",
                            TransactionDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            HoldingId = 5L,
                            CostBasis = 85.09m,
                            CurrentPrice = 0m,
                            ExpirationDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsOpen = false,
                            OrderType = "Buy",
                            PortfolioId = 3L,
                            Quantity = 500.0,
                            ReinvestDivs = false,
                            SecurityType = "Share",
                            StrikePrice = 0m,
                            Symbol = "ROKU",
                            TransactionDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("DegenApp.Models.Order", b =>
                {
                    b.Property<long>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<long>("PortfolioId")
                        .HasColumnType("bigint");

                    b.Property<decimal>("Price")
                        .HasColumnType("Money");

                    b.Property<double>("Quantity")
                        .HasColumnType("float");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.HasKey("OrderId");

                    b.HasIndex("PortfolioId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("DegenApp.Models.Portfolio", b =>
                {
                    b.Property<long>("PortfolioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("GainLoss")
                        .HasColumnType("Money");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalMarketValue")
                        .HasColumnType("Money");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PortfolioId");

                    b.ToTable("Portfolios");

                    b.HasData(
                        new
                        {
                            PortfolioId = 1L,
                            CreationDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            GainLoss = 768.00m,
                            Title = "User1's portfolio",
                            TotalMarketValue = 0m,
                            Type = "Investing",
                            UserId = "1"
                        },
                        new
                        {
                            PortfolioId = 2L,
                            CreationDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            GainLoss = -324.67m,
                            Title = "User1's Roth IRA Portfolio",
                            TotalMarketValue = 5204.99m,
                            Type = "Roth IRA",
                            UserId = "1"
                        },
                        new
                        {
                            PortfolioId = 3L,
                            CreationDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            GainLoss = 19874.73m,
                            Title = "User2's Primary Portfolio",
                            TotalMarketValue = 52064.29m,
                            Type = "Speculation",
                            UserId = "2"
                        });
                });

            modelBuilder.Entity("DegenApp.Models.WatchItem", b =>
                {
                    b.Property<long>("WatchItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<int>("Outlook")
                        .HasColumnType("int");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("WatchItemId");

                    b.ToTable("WatchItems");
                });

            modelBuilder.Entity("DegenApp.Models.Holding", b =>
                {
                    b.HasOne("DegenApp.Models.Portfolio", "Portfolio")
                        .WithMany("Holdings")
                        .HasForeignKey("PortfolioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Portfolio");
                });

            modelBuilder.Entity("DegenApp.Models.Order", b =>
                {
                    b.HasOne("DegenApp.Models.Portfolio", null)
                        .WithMany("Orders")
                        .HasForeignKey("PortfolioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DegenApp.Models.Portfolio", b =>
                {
                    b.Navigation("Holdings");

                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
