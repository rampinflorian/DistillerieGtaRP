using Microsoft.EntityFrameworkCore.Migrations;

namespace DistillerieGtaRP.Migrations
{
    public partial class DeliveryByAndCreatedBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pricings_Companies_CompanyId",
                table: "Pricings");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Pricings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pricings_Companies_CompanyId",
                table: "Pricings",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pricings_Companies_CompanyId",
                table: "Pricings");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Pricings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Pricings_Companies_CompanyId",
                table: "Pricings",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
