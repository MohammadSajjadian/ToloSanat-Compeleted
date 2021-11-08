using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class x8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "preTitle",
                table: "element4s",
                newName: "description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "description",
                table: "element4s",
                newName: "preTitle");
        }
    }
}
