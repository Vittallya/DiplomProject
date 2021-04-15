using Microsoft.EntityFrameworkCore.Migrations;

namespace AirClub.Migrations
{
    public partial class TransferInvertIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InvertedForId",
                table: "Transfers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_InvertedForId",
                table: "Transfers",
                column: "InvertedForId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transfers_InvertedForId",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "InvertedForId",
                table: "Transfers");
        }
    }
}
