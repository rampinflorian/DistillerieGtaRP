using Microsoft.EntityFrameworkCore.Migrations;

namespace DistillerieManzibar.Migrations
{
    public partial class ApplicationUserAccountStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountStatusType",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountStatusType",
                table: "AspNetUsers");
        }
    }
}
