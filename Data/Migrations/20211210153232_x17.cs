using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class x17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "contractPayers");

            migrationBuilder.DropTable(
                name: "transportationPayers");

            migrationBuilder.DropTable(
                name: "provinces");

            migrationBuilder.DropColumn(
                name: "contractDescription",
                table: "elementProps");

            migrationBuilder.DropColumn(
                name: "file",
                table: "elementProps");

            migrationBuilder.DropColumn(
                name: "price",
                table: "elementProps");

            migrationBuilder.RenameColumn(
                name: "transportationDescription",
                table: "elementProps",
                newName: "OrderDescription");

            migrationBuilder.AddColumn<bool>(
                name: "IsEmail",
                table: "projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSms",
                table: "projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEmail",
                table: "posts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSms",
                table: "posts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "price",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    trackingCode = table.Column<int>(type: "int", nullable: false),
                    payTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    price = table.Column<int>(type: "int", nullable: false),
                    IsAdminConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.id);
                    table.ForeignKey(
                        name: "FK_orders_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_orders_userId",
                table: "orders",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropColumn(
                name: "IsEmail",
                table: "projects");

            migrationBuilder.DropColumn(
                name: "IsSms",
                table: "projects");

            migrationBuilder.DropColumn(
                name: "IsEmail",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "IsSms",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "price",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "OrderDescription",
                table: "elementProps",
                newName: "transportationDescription");

            migrationBuilder.AddColumn<string>(
                name: "contractDescription",
                table: "elementProps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "file",
                table: "elementProps",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "price",
                table: "elementProps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "contractPayers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsAdminConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    payTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    price = table.Column<int>(type: "int", nullable: false),
                    trackingCode = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contractPayers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "provinces",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    price = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provinces", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transportationPayers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsAdminConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    payTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    price = table.Column<int>(type: "int", nullable: false),
                    provinceId = table.Column<int>(type: "int", nullable: false),
                    trackingCode = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transportationPayers", x => x.id);
                    table.ForeignKey(
                        name: "FK_transportationPayers_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_transportationPayers_provinces_provinceId",
                        column: x => x.provinceId,
                        principalTable: "provinces",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_transportationPayers_provinceId",
                table: "transportationPayers",
                column: "provinceId");

            migrationBuilder.CreateIndex(
                name: "IX_transportationPayers_userId",
                table: "transportationPayers",
                column: "userId");
        }
    }
}
