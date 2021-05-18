using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DegenApp.Migrations
{
    public partial class addholdingseeds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Holdings",
                columns: new[] { "HoldingId", "Action", "CostBasis", "CurrentPrice", "ExpirationDate", "IsOpen", "PortfolioId", "Quantity", "ReinvestDivs", "StrikePrice", "Symbol", "TransactionDate", "Type" },
                values: new object[,]
                {
                    { 1L, "buy", 20.22m, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 1L, 1000.0, true, 0m, "NKE", new DateTime(2020, 12, 19, 16, 19, 35, 525, DateTimeKind.Local).AddTicks(6349), "share" },
                    { 2L, "buy", 17.98m, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 1L, 3000.0, false, 0m, "SNAP", new DateTime(2020, 12, 19, 16, 19, 35, 525, DateTimeKind.Local).AddTicks(7294), "share" },
                    { 3L, "buy", 20.22m, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 2L, 70.0, false, 0m, "FSLR", new DateTime(2020, 12, 19, 16, 19, 35, 525, DateTimeKind.Local).AddTicks(7323), "share" },
                    { 4L, "buy", 160.22m, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 3L, 800.0, true, 0m, "SPOT", new DateTime(2020, 12, 19, 16, 19, 35, 525, DateTimeKind.Local).AddTicks(7340), "share" }
                });

            migrationBuilder.UpdateData(
                table: "Portfolios",
                keyColumn: "PortfolioId",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2020, 12, 19, 16, 19, 35, 522, DateTimeKind.Local).AddTicks(9707));

            migrationBuilder.UpdateData(
                table: "Portfolios",
                keyColumn: "PortfolioId",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2020, 12, 22, 21, 24, 20, 525, DateTimeKind.Local).AddTicks(3985));

            migrationBuilder.UpdateData(
                table: "Portfolios",
                keyColumn: "PortfolioId",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2021, 1, 11, 23, 42, 41, 525, DateTimeKind.Local).AddTicks(4105));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 4L);

            migrationBuilder.UpdateData(
                table: "Portfolios",
                keyColumn: "PortfolioId",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2020, 12, 19, 16, 1, 23, 475, DateTimeKind.Local).AddTicks(8630));

            migrationBuilder.UpdateData(
                table: "Portfolios",
                keyColumn: "PortfolioId",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2020, 12, 22, 21, 6, 8, 478, DateTimeKind.Local).AddTicks(2339));

            migrationBuilder.UpdateData(
                table: "Portfolios",
                keyColumn: "PortfolioId",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2021, 1, 11, 23, 24, 29, 478, DateTimeKind.Local).AddTicks(2454));
        }
    }
}
