using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DegenApp.Migrations
{
    public partial class addseedsandwatchitemkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WatchItems",
                columns: table => new
                {
                    WatchItemId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchItems", x => x.WatchItemId);
                });

            migrationBuilder.UpdateData(
                table: "Portfolios",
                keyColumn: "PortfolioId",
                keyValue: 1L,
                columns: new[] { "CreationDate", "UserId" },
                values: new object[] { new DateTime(2020, 12, 19, 16, 1, 23, 475, DateTimeKind.Local).AddTicks(8630), "1" });

            migrationBuilder.UpdateData(
                table: "Portfolios",
                keyColumn: "PortfolioId",
                keyValue: 2L,
                columns: new[] { "CreationDate", "UserId" },
                values: new object[] { new DateTime(2020, 12, 22, 21, 6, 8, 478, DateTimeKind.Local).AddTicks(2339), "1" });

            migrationBuilder.InsertData(
                table: "Portfolios",
                columns: new[] { "PortfolioId", "CreationDate", "GainLoss", "Title", "TotalMarketValue", "Type", "UserId" },
                values: new object[] { 3L, new DateTime(2021, 1, 11, 23, 24, 29, 478, DateTimeKind.Local).AddTicks(2454), 19874.73m, "User2's Primary Portfolio", 52064.29m, "Speculation", "2" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WatchItems");

            migrationBuilder.DeleteData(
                table: "Portfolios",
                keyColumn: "PortfolioId",
                keyValue: 3L);

            migrationBuilder.UpdateData(
                table: "Portfolios",
                keyColumn: "PortfolioId",
                keyValue: 1L,
                columns: new[] { "CreationDate", "UserId" },
                values: new object[] { new DateTime(2020, 12, 18, 22, 12, 8, 257, DateTimeKind.Local).AddTicks(6308), null });

            migrationBuilder.UpdateData(
                table: "Portfolios",
                keyColumn: "PortfolioId",
                keyValue: 2L,
                columns: new[] { "CreationDate", "UserId" },
                values: new object[] { new DateTime(2020, 12, 22, 3, 16, 53, 260, DateTimeKind.Local).AddTicks(5401), null });
        }
    }
}
