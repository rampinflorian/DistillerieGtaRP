using Microsoft.EntityFrameworkCore.Migrations;

namespace DistillerieManzibar.Migrations
{
    public partial class RelationTableCommands : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Commands_CommandId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CommandId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CommandId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "ApplicationUserCommand",
                columns: table => new
                {
                    ApplicationUsersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CommandsCommandId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserCommand", x => new { x.ApplicationUsersId, x.CommandsCommandId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserCommand_AspNetUsers_ApplicationUsersId",
                        column: x => x.ApplicationUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserCommand_Commands_CommandsCommandId",
                        column: x => x.CommandsCommandId,
                        principalTable: "Commands",
                        principalColumn: "CommandId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserCommand_CommandsCommandId",
                table: "ApplicationUserCommand",
                column: "CommandsCommandId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserCommand");

            migrationBuilder.AddColumn<int>(
                name: "CommandId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CommandId",
                table: "AspNetUsers",
                column: "CommandId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Commands_CommandId",
                table: "AspNetUsers",
                column: "CommandId",
                principalTable: "Commands",
                principalColumn: "CommandId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
