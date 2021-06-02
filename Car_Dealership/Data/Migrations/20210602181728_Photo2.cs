using Microsoft.EntityFrameworkCore.Migrations;

namespace Car_Dealership.Data.Migrations
{
    public partial class Photo2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo2",
                table: "Autos");

            migrationBuilder.DropColumn(
                name: "Photo3",
                table: "Autos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Photo2",
                table: "Autos",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Photo3",
                table: "Autos",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }
    }
}
