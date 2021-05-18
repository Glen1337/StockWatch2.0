﻿// <auto-generated />
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
    [Migration("20201219031208_attributes")]
    partial class attributes
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

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<decimal>("CostBasis")
                        .HasColumnType("Money");

                    b.Property<decimal>("CurrentPrice")
                        .HasColumnType("Money");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsOpen")
                        .HasColumnType("bit");

                    b.Property<long>("PortfolioId")
                        .HasColumnType("bigint");

                    b.Property<double>("Quantity")
                        .HasColumnType("float");

                    b.Property<bool>("ReinvestDivs")
                        .HasColumnType("bit");

                    b.Property<decimal>("StrikePrice")
                        .HasColumnType("Money");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.HasKey("HoldingId");

                    b.HasIndex("PortfolioId");

                    b.ToTable("Holdings");
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
                            CreationDate = new DateTime(2020, 12, 18, 22, 12, 8, 257, DateTimeKind.Local).AddTicks(6308),
                            GainLoss = 768.00m,
                            Title = "User1's portfolio",
                            TotalMarketValue = 1000.01m,
                            Type = "Investing"
                        },
                        new
                        {
                            PortfolioId = 2L,
                            CreationDate = new DateTime(2020, 12, 22, 3, 16, 53, 260, DateTimeKind.Local).AddTicks(5401),
                            GainLoss = -324.67m,
                            Title = "User1's Roth IRA Portfolio",
                            TotalMarketValue = 5204.99m,
                            Type = "Roth IRA"
                        });
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

            modelBuilder.Entity("DegenApp.Models.Portfolio", b =>
                {
                    b.Navigation("Holdings");
                });
#pragma warning restore 612, 618
        }
    }
}
