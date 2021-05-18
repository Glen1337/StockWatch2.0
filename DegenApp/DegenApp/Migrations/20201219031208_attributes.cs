using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DegenApp.Migrations
{
    public partial class attributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Portfolios",
                keyColumn: "PortfolioId",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2020, 12, 18, 22, 12, 8, 257, DateTimeKind.Local).AddTicks(6308));

            migrationBuilder.UpdateData(
                table: "Portfolios",
                keyColumn: "PortfolioId",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2020, 12, 22, 3, 16, 53, 260, DateTimeKind.Local).AddTicks(5401));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
