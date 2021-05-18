using Microsoft.EntityFrameworkCore.Migrations;

namespace DegenApp.Migrations
{
    public partial class InitialSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Portfolios",
                columns: new[] { "PortfolioId", "Title" },
                values: new object[] { 1L, "User2's portfolio" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Portfolios",
                keyColumn: "PortfolioId",
                keyValue: 1L);
        }
    }
}
