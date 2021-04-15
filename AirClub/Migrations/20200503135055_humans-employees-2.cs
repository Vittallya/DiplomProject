using Microsoft.EntityFrameworkCore.Migrations;

namespace AirClub.Migrations
{
    public partial class humansemployees2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Salary",
                table: "Specials",
                nullable: true);

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "AccessCode",
                table: "Humen",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "Humen",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Humen",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpecialId",
                table: "Humen",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Humen",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Humen_SpecialId",
                table: "Humen",
                column: "SpecialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Humen_Specials_SpecialId",
                table: "Humen",
                column: "SpecialId",
                principalTable: "Specials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salary",
                table: "Specials");

            migrationBuilder.DropForeignKey(
                name: "FK_Humen_Specials_SpecialId",
                table: "Humen");

            migrationBuilder.DropIndex(
                name: "IX_Humen_SpecialId",
                table: "Humen");

            migrationBuilder.DropColumn(
                name: "AccessCode",
                table: "Humen");

            migrationBuilder.DropColumn(
                name: "Login",
                table: "Humen");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Humen");

            migrationBuilder.DropColumn(
                name: "SpecialId",
                table: "Humen");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Humen");

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Specials_SpecialId",
                        column: x => x.SpecialId,
                        principalTable: "Specials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SpecialId",
                table: "Employees",
                column: "SpecialId");
        }
    }
}
