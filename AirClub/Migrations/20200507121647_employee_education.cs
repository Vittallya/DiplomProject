using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AirClub.Migrations
{
    public partial class employee_education : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EdDocGetDate",
                table: "Humen",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EducationDoc",
                table: "Humen",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EdDocGetDate",
                table: "Humen");

            migrationBuilder.DropColumn(
                name: "EducationDoc",
                table: "Humen");
        }
    }
}
