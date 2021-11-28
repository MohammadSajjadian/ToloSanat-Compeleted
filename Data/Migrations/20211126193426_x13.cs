using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class x13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "provinceId",
                table: "transportationPayers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_transportationPayers_provinceId",
                table: "transportationPayers",
                column: "provinceId");

            migrationBuilder.AddForeignKey(
                name: "FK_transportationPayers_provinces_provinceId",
                table: "transportationPayers",
                column: "provinceId",
                principalTable: "provinces",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transportationPayers_provinces_provinceId",
                table: "transportationPayers");

            migrationBuilder.DropIndex(
                name: "IX_transportationPayers_provinceId",
                table: "transportationPayers");

            migrationBuilder.DropColumn(
                name: "provinceId",
                table: "transportationPayers");
        }
    }
}
