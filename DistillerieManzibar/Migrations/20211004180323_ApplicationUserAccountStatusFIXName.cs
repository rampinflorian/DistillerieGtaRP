using Microsoft.EntityFrameworkCore.Migrations;

namespace DistillerieManzibar.Migrations
{
    public partial class ApplicationUserAccountStatusFIXName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountStatusType",
                table: "AspNetUsers",
                newName: "AccountStatus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountStatus",
                table: "AspNetUsers",
                newName: "AccountStatusType");
        }
    }
}
