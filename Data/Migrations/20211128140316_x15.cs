using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class x15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "price",
                table: "transportationPayers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "price",
                table: "contractPayers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "price",
                table: "transportationPayers");

            migrationBuilder.DropColumn(
                name: "price",
                table: "contractPayers");
        }
    }
}
