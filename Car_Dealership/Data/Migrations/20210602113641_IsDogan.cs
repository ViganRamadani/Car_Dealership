using Microsoft.EntityFrameworkCore.Migrations;

namespace Car_Dealership.Data.Migrations
{
    public partial class IsDogan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDogan",
                table: "Autos",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDogan",
                table: "Autos");
        }
    }
}
