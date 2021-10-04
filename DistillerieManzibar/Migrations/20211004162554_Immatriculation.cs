using Microsoft.EntityFrameworkCore.Migrations;

namespace DistillerieManzibar.Migrations
{
    public partial class Immatriculation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Registration",
                table: "Car",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Registration",
                table: "Car");
        }
    }
}
