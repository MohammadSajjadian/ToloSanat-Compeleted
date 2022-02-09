using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class x20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "e2ImageTitle",
                table: "elementProps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "extraDes1",
                table: "elementProps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "extraDes2",
                table: "elementProps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "projectImage",
                table: "elementProps",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "siteTopTab",
                table: "elementProps",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "e2ImageTitle",
                table: "elementProps");

            migrationBuilder.DropColumn(
                name: "extraDes1",
                table: "elementProps");

            migrationBuilder.DropColumn(
                name: "extraDes2",
                table: "elementProps");

            migrationBuilder.DropColumn(
                name: "projectImage",
                table: "elementProps");

            migrationBuilder.DropColumn(
                name: "siteTopTab",
                table: "elementProps");
        }
    }
}
