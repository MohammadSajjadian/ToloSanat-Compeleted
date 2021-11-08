using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class x6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "confirm",
                table: "inboxes",
                newName: "IsConfirm");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsConfirm",
                table: "inboxes",
                newName: "confirm");
        }
    }
}
