using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DegenApp.Migrations
{
    public partial class fixdatesholdingseed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 1L,
                column: "TransactionDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 2L,
                column: "TransactionDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 3L,
                column: "TransactionDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 4L,
                column: "TransactionDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 5L,
                column: "TransactionDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Portfolios",
                keyColumn: "PortfolioId",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Portfolios",
                keyColumn: "PortfolioId",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Portfolios",
                keyColumn: "PortfolioId",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 5L,
                column: "TransactionDate",
                value: new DateTime(2020, 12, 19, 16, 29, 9, 114, DateTimeKind.Local).AddTicks(2447));

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
    }
}
