using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AirClub.Migrations
{
    public partial class services : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    AgeFrom = table.Column<int>(nullable: false),
                    AgeBefore = table.Column<int>(nullable: false),
                    PhysReqs = table.Column<string>(nullable: true),
                    Cost = table.Column<double>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    TourId = table.Column<int>(nullable: true),
                    MinCountPeople = table.Column<int>(nullable: true),
                    MaxCountPeople = table.Column<int>(nullable: true),
                    DateBegin = table.Column<DateTime>(nullable: true),
                    DateEnd = table.Column<DateTime>(nullable: true),
                    CourseDuration = table.Column<int>(nullable: true),
                    ExersiceDuration = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Services");
        }
    }
}
