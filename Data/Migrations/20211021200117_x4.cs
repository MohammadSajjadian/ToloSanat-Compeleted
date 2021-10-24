using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class x4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "aboutUsLongDes",
                table: "elementProps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "aboutUsShortDes",
                table: "elementProps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "contactUsDescription",
                table: "elementProps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "contactUsTitle",
                table: "elementProps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "inboxes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nameFamily = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phonenumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inboxes", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inboxes");

            migrationBuilder.DropColumn(
                name: "aboutUsLongDes",
                table: "elementProps");

            migrationBuilder.DropColumn(
                name: "aboutUsShortDes",
                table: "elementProps");

            migrationBuilder.DropColumn(
                name: "contactUsDescription",
                table: "elementProps");

            migrationBuilder.DropColumn(
                name: "contactUsTitle",
                table: "elementProps");
        }
    }
}
