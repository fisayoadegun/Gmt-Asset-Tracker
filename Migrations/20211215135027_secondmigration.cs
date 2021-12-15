using Microsoft.EntityFrameworkCore.Migrations;

namespace Gmt_Asset_Tracker.Migrations
{
    public partial class secondmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Assets_PresentLocationId",
                table: "Assets",
                column: "PresentLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Locations_PresentLocationId",
                table: "Assets",
                column: "PresentLocationId",
                principalTable: "Locations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Locations_PresentLocationId",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_Assets_PresentLocationId",
                table: "Assets");
        }
    }
}
