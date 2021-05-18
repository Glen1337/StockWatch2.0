using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DegenApp.Migrations
{
    public partial class FixStrikeType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "StrikePrice",
                table: "Holdings",
                type: "Money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.UpdateData(
                table: "Portfolios",
                keyColumn: "PortfolioId",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2020, 12, 13, 23, 22, 47, 708, DateTimeKind.Local).AddTicks(6249));

            migrationBuilder.UpdateData(
                table: "Portfolios",
                keyColumn: "PortfolioId",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2020, 12, 17, 4, 27, 32, 710, DateTimeKind.Local).AddTicks(9492));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "StrikePrice",
                table: "Holdings",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Money");

            migrationBuilder.UpdateData(
                table: "Portfolios",
                keyColumn: "PortfolioId",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2020, 12, 13, 23, 21, 28, 262, DateTimeKind.Local).AddTicks(1786));

            migrationBuilder.UpdateData(
                table: "Portfolios",
                keyColumn: "PortfolioId",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2020, 12, 17, 4, 26, 13, 264, DateTimeKind.Local).AddTicks(5103));
        }
    }
}
