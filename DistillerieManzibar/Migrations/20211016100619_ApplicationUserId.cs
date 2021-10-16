using Microsoft.EntityFrameworkCore.Migrations;

namespace DistillerieManzibar.Migrations
{
    public partial class ApplicationUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_AspNetUsers_ApplicationUserId",
                table: "Transaction");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Transaction",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_AspNetUsers_ApplicationUserId",
                table: "Transaction",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_AspNetUsers_ApplicationUserId",
                table: "Transaction");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Transaction",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_AspNetUsers_ApplicationUserId",
                table: "Transaction",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
