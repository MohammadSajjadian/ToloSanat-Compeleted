using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class x14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "transportationPayers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "trackingCode",
                table: "transportationPayers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_transportationPayers_userId",
                table: "transportationPayers",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_transportationPayers_AspNetUsers_userId",
                table: "transportationPayers",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transportationPayers_AspNetUsers_userId",
                table: "transportationPayers");

            migrationBuilder.DropIndex(
                name: "IX_transportationPayers_userId",
                table: "transportationPayers");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "transportationPayers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "trackingCode",
                table: "transportationPayers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
