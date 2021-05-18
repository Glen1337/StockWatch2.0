using Microsoft.EntityFrameworkCore.Migrations;

namespace DegenApp.Migrations
{
    public partial class addPricePercentChangeToWatchItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PercentChange",
                table: "WatchItems",
                type: "decimal(12,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceChange",
                table: "WatchItems",
                type: "decimal(12,4)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PercentChange",
                table: "WatchItems");

            migrationBuilder.DropColumn(
                name: "PriceChange",
                table: "WatchItems");
        }
    }
}
