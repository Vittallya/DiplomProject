using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AirClub.Migrations
{
    public partial class UserActions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserActions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmloyeeId = table.Column<int>(nullable: false),
                    DateAction = table.Column<DateTime>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    ItemId = table.Column<int>(nullable: true),
                    ItemType = table.Column<string>(nullable: true),
                    ActionType = table.Column<int>(nullable: true),
                    NumberOfRertyes = table.Column<int>(nullable: true),
                    VmType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserActions_Humen_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Humen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserActions_EmployeeId",
                table: "UserActions",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserActions");
        }
    }
}
