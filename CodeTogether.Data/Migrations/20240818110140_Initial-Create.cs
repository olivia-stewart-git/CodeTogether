using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeTogether.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StmData",
                columns: table => new
                {
                    STM_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    STM_Key = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    STM_Value = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StmData", x => x.STM_PK);
                });

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    OT_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OT_AssemblyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OT_TypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.OT_PK);
                });

            migrationBuilder.CreateTable(
                name: "Scaffolds",
                columns: table => new
                {
                    EXE_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EXE_ScaffoldName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EXE_ScaffoldText = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false),
                    EXE_ExecutionRunnerName = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    EXE_ReturnTypeOT_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EXE_ExecutionRunnerArgument = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scaffolds", x => x.EXE_PK);
                    table.ForeignKey(
                        name: "FK_Scaffolds_Types_EXE_ReturnTypeOT_PK",
                        column: x => x.EXE_ReturnTypeOT_PK,
                        principalTable: "Types",
                        principalColumn: "OT_PK");
                });

            migrationBuilder.CreateTable(
                name: "Parameters",
                columns: table => new
                {
                    TC_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TC_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TC_TypeOT_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TC_Position = table.Column<int>(type: "int", nullable: false),
                    TC_ScaffoldEXE_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameters", x => x.TC_PK);
                    table.ForeignKey(
                        name: "FK_Parameters_Scaffolds_TC_ScaffoldEXE_PK",
                        column: x => x.TC_ScaffoldEXE_PK,
                        principalTable: "Scaffolds",
                        principalColumn: "EXE_PK",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Parameters_Types_TC_TypeOT_PK",
                        column: x => x.TC_TypeOT_PK,
                        principalTable: "Types",
                        principalColumn: "OT_PK",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    QST_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QST_Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    QST_Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    QST_ScaffoldEXE_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QST_PK);
                    table.ForeignKey(
                        name: "FK_Questions_Scaffolds_QST_ScaffoldEXE_PK",
                        column: x => x.QST_ScaffoldEXE_PK,
                        principalTable: "Scaffolds",
                        principalColumn: "EXE_PK",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestCases",
                columns: table => new
                {
                    TST_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TST_IsHidden = table.Column<bool>(type: "bit", nullable: false),
                    TST_Title = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TST_Arguments = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TST_ExpectedResponse = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TST_QST_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestCases", x => x.TST_PK);
                    table.ForeignKey(
                        name: "FK_TestCases_Questions_TST_QST_FK",
                        column: x => x.TST_QST_FK,
                        principalTable: "Questions",
                        principalColumn: "QST_PK");
                });

            migrationBuilder.CreateTable(
                name: "GamePlayers",
                columns: table => new
                {
                    GMP_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GMP_GM_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GMP_USR_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlayers", x => x.GMP_PK);
                });

            migrationBuilder.CreateTable(
                name: "Submissions",
                columns: table => new
                {
                    SBM_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SBM_SubmissionStartTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SBM_SubmissionDuration = table.Column<TimeSpan>(type: "time", nullable: false),
                    SBM_Code = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false),
                    SBM_QuestionQST_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SBM_SubmittedByGMP_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SBM_Status = table.Column<int>(type: "int", nullable: false),
                    SBM_CompileError = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.SBM_PK);
                    table.ForeignKey(
                        name: "FK_Submissions_GamePlayers_SBM_SubmittedByGMP_PK",
                        column: x => x.SBM_SubmittedByGMP_PK,
                        principalTable: "GamePlayers",
                        principalColumn: "GMP_PK",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Submissions_Questions_SBM_QuestionQST_PK",
                        column: x => x.SBM_QuestionQST_PK,
                        principalTable: "Questions",
                        principalColumn: "QST_PK",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    USR_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    USR_Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    USR_UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    USR_PasswordHash = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    USR_PasswordSalt = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    USR_CheckPoints = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    USR_LastHeardFromAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    USR_GMP_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.USR_PK);
                    table.ForeignKey(
                        name: "FK_Users_GamePlayers_USR_GMP_FK",
                        column: x => x.USR_GMP_FK,
                        principalTable: "GamePlayers",
                        principalColumn: "GMP_PK");
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    GM_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GM_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GM_CreateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GM_StartedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GM_FinishedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GM_Private = table.Column<bool>(type: "bit", nullable: false),
                    GM_MaxPlayers = table.Column<int>(type: "int", nullable: false),
                    GM_WaitForAll = table.Column<bool>(type: "bit", nullable: false),
                    GM_CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GM_GM_NextGame_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GM_WinningSubmissionSBM_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GM_QST_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.GM_PK);
                    table.ForeignKey(
                        name: "FK_Games_Games_GM_GM_NextGame_FK",
                        column: x => x.GM_GM_NextGame_FK,
                        principalTable: "Games",
                        principalColumn: "GM_PK");
                    table.ForeignKey(
                        name: "FK_Games_Questions_GM_QST_FK",
                        column: x => x.GM_QST_FK,
                        principalTable: "Questions",
                        principalColumn: "QST_PK");
                    table.ForeignKey(
                        name: "FK_Games_Submissions_GM_WinningSubmissionSBM_PK",
                        column: x => x.GM_WinningSubmissionSBM_PK,
                        principalTable: "Submissions",
                        principalColumn: "SBM_PK");
                });

            migrationBuilder.CreateTable(
                name: "TestRuns",
                columns: table => new
                {
                    TCR_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TCR_Status = table.Column<int>(type: "int", nullable: false),
                    TCR_ActualResult = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false),
                    TCR_Exception = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    TCR_ParentTST_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TCR_SubmissionResultSBM_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestRuns", x => x.TCR_PK);
                    table.ForeignKey(
                        name: "FK_TestRuns_Submissions_TCR_SubmissionResultSBM_PK",
                        column: x => x.TCR_SubmissionResultSBM_PK,
                        principalTable: "Submissions",
                        principalColumn: "SBM_PK",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestRuns_TestCases_TCR_ParentTST_PK",
                        column: x => x.TCR_ParentTST_PK,
                        principalTable: "TestCases",
                        principalColumn: "TST_PK",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayers_GMP_GM_FK",
                table: "GamePlayers",
                column: "GMP_GM_FK");

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayers_GMP_USR_FK",
                table: "GamePlayers",
                column: "GMP_USR_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Games_GM_GM_NextGame_FK",
                table: "Games",
                column: "GM_GM_NextGame_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Games_GM_QST_FK",
                table: "Games",
                column: "GM_QST_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Games_GM_WinningSubmissionSBM_PK",
                table: "Games",
                column: "GM_WinningSubmissionSBM_PK");

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_TC_ScaffoldEXE_PK",
                table: "Parameters",
                column: "TC_ScaffoldEXE_PK");

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_TC_TypeOT_PK",
                table: "Parameters",
                column: "TC_TypeOT_PK");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QST_ScaffoldEXE_PK",
                table: "Questions",
                column: "QST_ScaffoldEXE_PK");

            migrationBuilder.CreateIndex(
                name: "IX_Scaffolds_EXE_ReturnTypeOT_PK",
                table: "Scaffolds",
                column: "EXE_ReturnTypeOT_PK");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_SBM_QuestionQST_PK",
                table: "Submissions",
                column: "SBM_QuestionQST_PK");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_SBM_SubmittedByGMP_PK",
                table: "Submissions",
                column: "SBM_SubmittedByGMP_PK");

            migrationBuilder.CreateIndex(
                name: "IX_TestCases_TST_QST_FK",
                table: "TestCases",
                column: "TST_QST_FK");

            migrationBuilder.CreateIndex(
                name: "IX_TestRuns_TCR_ParentTST_PK",
                table: "TestRuns",
                column: "TCR_ParentTST_PK");

            migrationBuilder.CreateIndex(
                name: "IX_TestRuns_TCR_SubmissionResultSBM_PK",
                table: "TestRuns",
                column: "TCR_SubmissionResultSBM_PK");

            migrationBuilder.CreateIndex(
                name: "IX_Users_USR_GMP_FK",
                table: "Users",
                column: "USR_GMP_FK");

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

            migrationBuilder.DropTable(
                name: "Parameters");

            migrationBuilder.DropTable(
                name: "StmData");

            migrationBuilder.DropTable(
                name: "TestRuns");

            migrationBuilder.DropTable(
                name: "TestCases");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Submissions");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Scaffolds");

            migrationBuilder.DropTable(
                name: "Types");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "GamePlayers");
        }
    }
}
