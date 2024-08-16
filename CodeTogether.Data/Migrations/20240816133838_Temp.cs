using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeTogether.Data.Migrations
{
    /// <inheritdoc />
    public partial class Temp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamePlayers_Games_GMP_GM_FK",
                table: "GamePlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_GamePlayers_Users_GMP_USR_FK",
                table: "GamePlayers");

            migrationBuilder.DropTable(
                name: "GameModelUserModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GamePlayers",
                table: "GamePlayers");

            migrationBuilder.AddColumn<Guid>(
                name: "USR_GMP_FK",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TC_Name",
                table: "Parameters",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "GMP_PK",
                table: "GamePlayers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_GamePlayers",
                table: "GamePlayers",
                column: "GMP_PK");

            migrationBuilder.CreateTable(
                name: "CompletedSubmissions",
                columns: table => new
                {
                    CSM_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CSM_EXR_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CSM_CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CSM_GM_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CSM_USR_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedSubmissions", x => x.CSM_PK);
                    table.ForeignKey(
                        name: "FK_CompletedSubmissions_Games_CSM_GM_FK",
                        column: x => x.CSM_GM_FK,
                        principalTable: "Games",
                        principalColumn: "GM_PK");
                    table.ForeignKey(
                        name: "FK_CompletedSubmissions_SubmissionResults_CSM_EXR_FK",
                        column: x => x.CSM_EXR_FK,
                        principalTable: "SubmissionResults",
                        principalColumn: "EXR_PK");
                    table.ForeignKey(
                        name: "FK_CompletedSubmissions_Users_CSM_USR_FK",
                        column: x => x.CSM_USR_FK,
                        principalTable: "Users",
                        principalColumn: "USR_PK");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_USR_GMP_FK",
                table: "Users",
                column: "USR_GMP_FK");

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayers_GMP_GM_FK",
                table: "GamePlayers",
                column: "GMP_GM_FK");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedSubmissions_CSM_EXR_FK",
                table: "CompletedSubmissions",
                column: "CSM_EXR_FK");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedSubmissions_CSM_GM_FK",
                table: "CompletedSubmissions",
                column: "CSM_GM_FK");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedSubmissions_CSM_USR_FK",
                table: "CompletedSubmissions",
                column: "CSM_USR_FK");

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlayers_Games_GMP_GM_FK",
                table: "GamePlayers",
                column: "GMP_GM_FK",
                principalTable: "Games",
                principalColumn: "GM_PK");

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlayers_Users_GMP_USR_FK",
                table: "GamePlayers",
                column: "GMP_USR_FK",
                principalTable: "Users",
                principalColumn: "USR_PK");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_GamePlayers_USR_GMP_FK",
                table: "Users",
                column: "USR_GMP_FK",
                principalTable: "GamePlayers",
                principalColumn: "GMP_PK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamePlayers_Games_GMP_GM_FK",
                table: "GamePlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_GamePlayers_Users_GMP_USR_FK",
                table: "GamePlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_GamePlayers_USR_GMP_FK",
                table: "Users");

            migrationBuilder.DropTable(
                name: "CompletedSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_Users_USR_GMP_FK",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GamePlayers",
                table: "GamePlayers");

            migrationBuilder.DropIndex(
                name: "IX_GamePlayers_GMP_GM_FK",
                table: "GamePlayers");

            migrationBuilder.DropColumn(
                name: "USR_GMP_FK",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "GMP_PK",
                table: "GamePlayers");

            migrationBuilder.AlterColumn<string>(
                name: "TC_Name",
                table: "Parameters",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GamePlayers",
                table: "GamePlayers",
                columns: new[] { "GMP_GM_FK", "GMP_USR_FK" });

            migrationBuilder.CreateTable(
                name: "GameModelUserModel",
                columns: table => new
                {
                    GamesGM_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersUSR_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameModelUserModel", x => new { x.GamesGM_PK, x.UsersUSR_PK });
                    table.ForeignKey(
                        name: "FK_GameModelUserModel_Games_GamesGM_PK",
                        column: x => x.GamesGM_PK,
                        principalTable: "Games",
                        principalColumn: "GM_PK",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameModelUserModel_Users_UsersUSR_PK",
                        column: x => x.UsersUSR_PK,
                        principalTable: "Users",
                        principalColumn: "USR_PK",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameModelUserModel_UsersUSR_PK",
                table: "GameModelUserModel",
                column: "UsersUSR_PK");

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlayers_Games_GMP_GM_FK",
                table: "GamePlayers",
                column: "GMP_GM_FK",
                principalTable: "Games",
                principalColumn: "GM_PK",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlayers_Users_GMP_USR_FK",
                table: "GamePlayers",
                column: "GMP_USR_FK",
                principalTable: "Users",
                principalColumn: "USR_PK",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
