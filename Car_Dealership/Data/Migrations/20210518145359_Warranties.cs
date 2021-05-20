using Microsoft.EntityFrameworkCore.Migrations;

namespace Car_Dealership.Data.Migrations
{
    public partial class Warranties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Warranties",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    warrantyName = table.Column<string>(nullable: true),
                    warrantyYears = table.Column<int>(nullable: false),
                    warrantyCoverage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warranties", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Warranties");
        }
    }
}
