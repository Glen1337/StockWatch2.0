using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DegenApp.Migrations
{
    public partial class switchfromstringstoattributesinholdings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                table: "Holdings");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Holdings");

            migrationBuilder.AddColumn<int>(
                name: "OrderType",
                table: "Holdings",
                type: "int",
                maxLength: 16,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SecurityType",
                table: "Holdings",
                type: "int",
                maxLength: 8,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Action = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<decimal>(type: "Money", nullable: false),
                    PortfolioId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "PortfolioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 1L,
                column: "SecurityType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 2L,
                column: "SecurityType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 3L,
                column: "SecurityType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 4L,
                column: "SecurityType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 5L,
                column: "SecurityType",
                value: 2);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PortfolioId",
                table: "Orders",
                column: "PortfolioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderType",
                table: "Holdings");

            migrationBuilder.DropColumn(
                name: "SecurityType",
                table: "Holdings");

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "Holdings",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Holdings",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 1L,
                columns: new[] { "Action", "Type" },
                values: new object[] { "buy", "share" });

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 2L,
                columns: new[] { "Action", "Type" },
                values: new object[] { "buy", "share" });

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 3L,
                columns: new[] { "Action", "Type" },
                values: new object[] { "buy", "share" });

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 4L,
                columns: new[] { "Action", "Type" },
                values: new object[] { "buy", "share" });

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 5L,
                columns: new[] { "Action", "Type" },
                values: new object[] { "shortsell", "share" });
        }
    }
}
