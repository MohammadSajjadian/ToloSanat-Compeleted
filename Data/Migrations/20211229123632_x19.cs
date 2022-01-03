using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class x19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "logo",
                table: "elementProps",
                newName: "secondaryLogo");

            migrationBuilder.AddColumn<byte[]>(
                name: "mainLogo",
                table: "elementProps",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "mainLogo",
                table: "elementProps");

            migrationBuilder.RenameColumn(
                name: "secondaryLogo",
                table: "elementProps",
                newName: "logo");
        }
    }
}
