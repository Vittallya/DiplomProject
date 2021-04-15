using Microsoft.EntityFrameworkCore.Migrations;

namespace AirClub.Migrations
{
    public partial class clients : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasportData",
                table: "Humen",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasportData",
                table: "Humen");
        }
    }
}
