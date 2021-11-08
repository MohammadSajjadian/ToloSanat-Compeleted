using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class x7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "img",
                table: "element3s");

            migrationBuilder.DropColumn(
                name: "img",
                table: "element2s");

            migrationBuilder.AddColumn<byte[]>(
                name: "e2img",
                table: "elementProps",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "e3img",
                table: "elementProps",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "e4img",
                table: "elementProps",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "e2img",
                table: "elementProps");

            migrationBuilder.DropColumn(
                name: "e3img",
                table: "elementProps");

            migrationBuilder.DropColumn(
                name: "e4img",
                table: "elementProps");

            migrationBuilder.AddColumn<byte[]>(
                name: "img",
                table: "element3s",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "img",
                table: "element2s",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
