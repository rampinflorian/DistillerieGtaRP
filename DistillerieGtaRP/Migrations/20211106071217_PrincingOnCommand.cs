using Microsoft.EntityFrameworkCore.Migrations;

namespace DistillerieGtaRP.Migrations
{
    public partial class PrincingOnCommand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PricingId",
                table: "Commands",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricingId",
                table: "Commands");
        }
    }
}
