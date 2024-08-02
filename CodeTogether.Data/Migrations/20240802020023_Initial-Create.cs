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
                name: "ArgumentCollections",
                columns: table => new
                {
                    TC_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TC_TO_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArgumentCollections", x => x.TC_PK);
                });

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
                name: "TestExecutions",
                columns: table => new
                {
                    TRX_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestExecutions", x => x.TRX_PK);
                });

            migrationBuilder.CreateTable(
                name: "Arguments",
                columns: table => new
                {
                    OT_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OT_AssemblyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OT_TypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TC_TO_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arguments", x => x.OT_PK);
                    table.ForeignKey(
                        name: "FK_Arguments_ArgumentCollections_TC_TO_FK",
                        column: x => x.TC_TO_FK,
                        principalTable: "ArgumentCollections",
                        principalColumn: "TC_PK");
                });

            migrationBuilder.CreateTable(
                name: "ExecutionResults",
                columns: table => new
                {
                    EXR_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EXR_Status = table.Column<int>(type: "int", nullable: false),
                    EXR_TRX_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EXR_Exception = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionResults", x => x.EXR_PK);
                    table.ForeignKey(
                        name: "FK_ExecutionResults_TestExecutions_EXR_TRX_FK",
                        column: x => x.EXR_TRX_FK,
                        principalTable: "TestExecutions",
                        principalColumn: "TRX_PK");
                });

            migrationBuilder.CreateTable(
                name: "ExecutionConfigurations",
                columns: table => new
                {
                    EXE_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EXE_ScaffoldName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EXE_ExecutionRunnerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EXE_ExecutionArgument = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EXE_TC_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EXE_TO_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionConfigurations", x => x.EXE_PK);
                    table.ForeignKey(
                        name: "FK_ExecutionConfigurations_ArgumentCollections_EXE_TC_FK",
                        column: x => x.EXE_TC_FK,
                        principalTable: "ArgumentCollections",
                        principalColumn: "TC_PK");
                    table.ForeignKey(
                        name: "FK_ExecutionConfigurations_Arguments_EXE_TO_FK",
                        column: x => x.EXE_TO_FK,
                        principalTable: "Arguments",
                        principalColumn: "OT_PK");
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    QST_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QST_Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    QST_Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    QST_EXE_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QST_PK);
                    table.ForeignKey(
                        name: "FK_Questions_ExecutionConfigurations_QST_EXE_FK",
                        column: x => x.QST_EXE_FK,
                        principalTable: "ExecutionConfigurations",
                        principalColumn: "EXE_PK",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Submissions",
                columns: table => new
                {
                    SBM_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SBM_SubmissionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SBM_Code = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false),
                    SBM_EXR_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SBM_QST_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.SBM_PK);
                    table.ForeignKey(
                        name: "FK_Submissions_ExecutionResults_SBM_EXR_FK",
                        column: x => x.SBM_EXR_FK,
                        principalTable: "ExecutionResults",
                        principalColumn: "EXR_PK");
                    table.ForeignKey(
                        name: "FK_Submissions_Questions_SBM_QST_FK",
                        column: x => x.SBM_QST_FK,
                        principalTable: "Questions",
                        principalColumn: "QST_PK",
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
                    TST_QST_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                name: "TestRuns",
                columns: table => new
                {
                    TCR_PK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TCR_Status = table.Column<int>(type: "int", nullable: false),
                    TCR_ActualResult = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TCR_Exception = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    TCR_TST_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TCR_TRX_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestRuns", x => x.TCR_PK);
                    table.ForeignKey(
                        name: "FK_TestRuns_TestCases_TCR_TST_FK",
                        column: x => x.TCR_TST_FK,
                        principalTable: "TestCases",
                        principalColumn: "TST_PK",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestRuns_TestExecutions_TCR_TST_FK",
                        column: x => x.TCR_TST_FK,
                        principalTable: "TestExecutions",
                        principalColumn: "TRX_PK",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Arguments_TC_TO_FK",
                table: "Arguments",
                column: "TC_TO_FK");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionConfigurations_EXE_TC_FK",
                table: "ExecutionConfigurations",
                column: "EXE_TC_FK");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionConfigurations_EXE_TO_FK",
                table: "ExecutionConfigurations",
                column: "EXE_TO_FK");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionResults_EXR_TRX_FK",
                table: "ExecutionResults",
                column: "EXR_TRX_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QST_EXE_FK",
                table: "Questions",
                column: "QST_EXE_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_SBM_EXR_FK",
                table: "Submissions",
                column: "SBM_EXR_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_SBM_QST_FK",
                table: "Submissions",
                column: "SBM_QST_FK");

            migrationBuilder.CreateIndex(
                name: "IX_TestCases_TST_QST_FK",
                table: "TestCases",
                column: "TST_QST_FK");

            migrationBuilder.CreateIndex(
                name: "IX_TestRuns_TCR_TST_FK",
                table: "TestRuns",
                column: "TCR_TST_FK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StmData");

            migrationBuilder.DropTable(
                name: "Submissions");

            migrationBuilder.DropTable(
                name: "TestRuns");

            migrationBuilder.DropTable(
                name: "ExecutionResults");

            migrationBuilder.DropTable(
                name: "TestCases");

            migrationBuilder.DropTable(
                name: "TestExecutions");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "ExecutionConfigurations");

            migrationBuilder.DropTable(
                name: "Arguments");

            migrationBuilder.DropTable(
                name: "ArgumentCollections");
        }
    }
}
