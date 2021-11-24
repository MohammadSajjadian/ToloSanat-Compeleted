using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class x10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleteForAdmin",
                table: "messages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleteForAdmin",
                table: "groups",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleteForAdmin",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "IsDeleteForAdmin",
                table: "groups");
        }
    }
}
