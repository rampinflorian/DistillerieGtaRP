using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DistillerieManzibar.Data.Migrations
{
    public partial class CarLogRenameDirectionAndCar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarLogs",
                columns: table => new
                {
                    CarLogId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApplicationUserId = table.Column<string>(type: "TEXT", nullable: true),
                    CarDirection = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CarId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarLogs", x => x.CarLogId);
                    table.ForeignKey(
                        name: "FK_CarLogs_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CarLogs_Car_CarId",
                        column: x => x.CarId,
                        principalTable: "Car",
                        principalColumn: "CarId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarLogs_ApplicationUserId",
                table: "CarLogs",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CarLogs_CarId",
                table: "CarLogs",
                column: "CarId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarLogs");
        }
    }
}
