using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class x11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "contractDescription",
                table: "elementProps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "transportationDescription",
                table: "elementProps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "contractPayers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    trackingCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    payTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAdminConfirmed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contractPayers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transportationPayers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    trackingCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    payTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAdminConfirmed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transportationPayers", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "contractPayers");

            migrationBuilder.DropTable(
                name: "transportationPayers");

            migrationBuilder.DropColumn(
                name: "contractDescription",
                table: "elementProps");

            migrationBuilder.DropColumn(
                name: "transportationDescription",
                table: "elementProps");
        }
    }
}
