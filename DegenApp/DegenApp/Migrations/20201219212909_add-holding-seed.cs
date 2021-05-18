using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DegenApp.Migrations
{
    public partial class addholdingseed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 1L,
                column: "TransactionDate",
                value: new DateTime(2020, 12, 19, 16, 29, 9, 112, DateTimeKind.Local).AddTicks(6865));

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 2L,
                column: "TransactionDate",
                value: new DateTime(2020, 12, 19, 16, 29, 9, 114, DateTimeKind.Local).AddTicks(2371));

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 3L,
                column: "TransactionDate",
                value: new DateTime(2020, 12, 19, 16, 29, 9, 114, DateTimeKind.Local).AddTicks(2415));

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 4L,
                column: "TransactionDate",
                value: new DateTime(2020, 12, 19, 16, 29, 9, 114, DateTimeKind.Local).AddTicks(2433));

            migrationBuilder.InsertData(
                table: "Holdings",
                columns: new[] { "HoldingId", "Action", "CostBasis", "CurrentPrice", "ExpirationDate", "IsOpen", "PortfolioId", "Quantity", "ReinvestDivs", "StrikePrice", "Symbol", "TransactionDate", "Type" },
                values: new object[] { 5L, "shortsell", 85.09m, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 3L, 500.0, false, 0m, "ROKU", new DateTime(2020, 12, 19, 16, 29, 9, 114, DateTimeKind.Local).AddTicks(2447), "share" });

            migrationBuilder.UpdateData(
                table: "Portfolios",
                keyColumn: "PortfolioId",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2020, 12, 19, 16, 19, 35, 522, DateTimeKind.Local).AddTicks(6707));

            migrationBuilder.UpdateData(
                table: "Portfolios",
                keyColumn: "PortfolioId",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2020, 12, 19, 16, 19, 35, 522, DateTimeKind.Local).AddTicks(3249));

            migrationBuilder.UpdateData(
                table: "Portfolios",
                keyColumn: "PortfolioId",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2020, 12, 19, 16, 19, 35, 522, DateTimeKind.Local).AddTicks(2673));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 5L);

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 1L,
                column: "TransactionDate",
                value: new DateTime(2020, 12, 19, 16, 19, 35, 525, DateTimeKind.Local).AddTicks(6349));

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 2L,
                column: "TransactionDate",
                value: new DateTime(2020, 12, 19, 16, 19, 35, 525, DateTimeKind.Local).AddTicks(7294));

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 3L,
                column: "TransactionDate",
                value: new DateTime(2020, 12, 19, 16, 19, 35, 525, DateTimeKind.Local).AddTicks(7323));

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 4L,
                column: "TransactionDate",
                value: new DateTime(2020, 12, 19, 16, 19, 35, 525, DateTimeKind.Local).AddTicks(7340));

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
    }
}
