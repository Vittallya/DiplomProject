using Microsoft.EntityFrameworkCore.Migrations;

namespace AirClub.Migrations
{
    public partial class ReservationToInsurance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Partners_PartnerId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_PartnerId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "PartnerId",
                table: "Reservations");

            migrationBuilder.AddColumn<int>(
                name: "InsuranceId",
                table: "Reservations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_InsuranceId",
                table: "Reservations",
                column: "InsuranceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Insurances_InsuranceId",
                table: "Reservations",
                column: "InsuranceId",
                principalTable: "Insurances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Insurances_InsuranceId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_InsuranceId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "InsuranceId",
                table: "Reservations");

            migrationBuilder.AddColumn<int>(
                name: "PartnerId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_PartnerId",
                table: "Reservations",
                column: "PartnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Partners_PartnerId",
                table: "Reservations",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
