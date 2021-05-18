using Microsoft.EntityFrameworkCore.Migrations;

namespace DegenApp.Migrations
{
    public partial class storeenumsasstrings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SecurityType",
                table: "Holdings",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 8);

            migrationBuilder.AlterColumn<string>(
                name: "OrderType",
                table: "Holdings",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 16);

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 1L,
                columns: new[] { "OrderType", "SecurityType" },
                values: new object[] { "Buy", "Share" });

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 2L,
                columns: new[] { "OrderType", "SecurityType" },
                values: new object[] { "Buy", "Share" });

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 3L,
                columns: new[] { "OrderType", "SecurityType" },
                values: new object[] { "Buy", "Share" });

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 4L,
                columns: new[] { "OrderType", "SecurityType" },
                values: new object[] { "Buy", "Share" });

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 5L,
                columns: new[] { "OrderType", "SecurityType" },
                values: new object[] { "Buy", "Share" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SecurityType",
                table: "Holdings",
                type: "int",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(8)",
                oldMaxLength: 8);

            migrationBuilder.AlterColumn<int>(
                name: "OrderType",
                table: "Holdings",
                type: "int",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(16)",
                oldMaxLength: 16);

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 1L,
                columns: new[] { "OrderType", "SecurityType" },
                values: new object[] { 0, 2 });

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 2L,
                columns: new[] { "OrderType", "SecurityType" },
                values: new object[] { 0, 2 });

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 3L,
                columns: new[] { "OrderType", "SecurityType" },
                values: new object[] { 0, 2 });

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 4L,
                columns: new[] { "OrderType", "SecurityType" },
                values: new object[] { 0, 2 });

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "HoldingId",
                keyValue: 5L,
                columns: new[] { "OrderType", "SecurityType" },
                values: new object[] { 0, 2 });
        }
    }
}
