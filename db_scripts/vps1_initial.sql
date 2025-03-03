CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "StmData" (
    "STM_PK" uuid NOT NULL,
    "STM_Key" character varying(20) NOT NULL,
    "STM_Value" character varying(100) NOT NULL,
    CONSTRAINT "PK_StmData" PRIMARY KEY ("STM_PK")
);

CREATE TABLE "Types" (
    "OT_PK" uuid NOT NULL,
    "OT_AssemblyName" character varying(500) NOT NULL,
    "OT_TypeName" character varying(1000) NOT NULL,
    CONSTRAINT "PK_Types" PRIMARY KEY ("OT_PK")
);

CREATE TABLE "Scaffolds" (
    "EXE_PK" uuid NOT NULL,
    "EXE_ScaffoldName" character varying(1000) NOT NULL,
    "EXE_ScaffoldText" text NOT NULL,
    "EXE_ExecutionRunnerName" integer NOT NULL,
    "EXE_ReturnTypeOT_PK" uuid NOT NULL,
    "EXE_ExecutionRunnerArgument" character varying(100) NOT NULL,
    CONSTRAINT "PK_Scaffolds" PRIMARY KEY ("EXE_PK"),
    CONSTRAINT "FK_Scaffolds_Types_EXE_ReturnTypeOT_PK" FOREIGN KEY ("EXE_ReturnTypeOT_PK") REFERENCES "Types" ("OT_PK")
);

CREATE TABLE "Parameters" (
    "TC_PK" uuid NOT NULL,
    "TC_Name" character varying(30) NOT NULL,
    "TC_TypeOT_PK" uuid NOT NULL,
    "TC_Position" integer NOT NULL,
    "TC_ScaffoldEXE_PK" uuid NOT NULL,
    CONSTRAINT "PK_Parameters" PRIMARY KEY ("TC_PK"),
    CONSTRAINT "FK_Parameters_Scaffolds_TC_ScaffoldEXE_PK" FOREIGN KEY ("TC_ScaffoldEXE_PK") REFERENCES "Scaffolds" ("EXE_PK") ON DELETE CASCADE,
    CONSTRAINT "FK_Parameters_Types_TC_TypeOT_PK" FOREIGN KEY ("TC_TypeOT_PK") REFERENCES "Types" ("OT_PK") ON DELETE CASCADE
);

CREATE TABLE "Questions" (
    "QST_PK" uuid NOT NULL,
    "QST_Name" character varying(100) NOT NULL,
    "QST_Description" text NOT NULL,
    "QST_ScaffoldEXE_PK" uuid NOT NULL,
    CONSTRAINT "PK_Questions" PRIMARY KEY ("QST_PK"),
    CONSTRAINT "FK_Questions_Scaffolds_QST_ScaffoldEXE_PK" FOREIGN KEY ("QST_ScaffoldEXE_PK") REFERENCES "Scaffolds" ("EXE_PK") ON DELETE CASCADE
);

CREATE TABLE "TestCases" (
    "TST_PK" uuid NOT NULL,
    "TST_IsHidden" boolean NOT NULL,
    "TST_Title" character varying(50) NOT NULL,
    "TST_Arguments" text NOT NULL,
    "TST_ExpectedResponse" character varying(200) NOT NULL,
    "TST_QST_FK" uuid NOT NULL,
    CONSTRAINT "PK_TestCases" PRIMARY KEY ("TST_PK"),
    CONSTRAINT "FK_TestCases_Questions_TST_QST_FK" FOREIGN KEY ("TST_QST_FK") REFERENCES "Questions" ("QST_PK")
);

CREATE TABLE "GamePlayers" (
    "GMP_PK" uuid NOT NULL,
    "GMP_GM_FK" uuid NOT NULL,
    "GMP_USR_FK" uuid NOT NULL,
    CONSTRAINT "PK_GamePlayers" PRIMARY KEY ("GMP_PK")
);

CREATE TABLE "Submissions" (
    "SBM_PK" uuid NOT NULL,
    "SBM_SubmissionStartTimeUtc" timestamp with time zone NOT NULL,
    "SBM_SubmissionDuration" interval NOT NULL,
    "SBM_Code" text NOT NULL,
    "SBM_QuestionQST_PK" uuid NOT NULL,
    "SBM_SubmittedByGMP_PK" uuid NOT NULL,
    "SBM_Status" integer NOT NULL,
    "SBM_CompileError" text,
    CONSTRAINT "PK_Submissions" PRIMARY KEY ("SBM_PK"),
    CONSTRAINT "FK_Submissions_GamePlayers_SBM_SubmittedByGMP_PK" FOREIGN KEY ("SBM_SubmittedByGMP_PK") REFERENCES "GamePlayers" ("GMP_PK") ON DELETE CASCADE,
    CONSTRAINT "FK_Submissions_Questions_SBM_QuestionQST_PK" FOREIGN KEY ("SBM_QuestionQST_PK") REFERENCES "Questions" ("QST_PK") ON DELETE CASCADE
);

CREATE TABLE "Users" (
    "USR_PK" uuid NOT NULL,
    "USR_Email" character varying(100) NOT NULL,
    "USR_UserName" character varying(100) NOT NULL,
    "USR_PasswordHash" character varying(150) NOT NULL,
    "USR_PasswordSalt" character varying(150) NOT NULL,
    "USR_CheckPoints" text NOT NULL,
    "USR_LastHeardFromAt" timestamp with time zone NOT NULL,
    "USR_GMP_FK" uuid,
    CONSTRAINT "PK_Users" PRIMARY KEY ("USR_PK"),
    CONSTRAINT "FK_Users_GamePlayers_USR_GMP_FK" FOREIGN KEY ("USR_GMP_FK") REFERENCES "GamePlayers" ("GMP_PK")
);

CREATE TABLE "Games" (
    "GM_PK" uuid NOT NULL,
    "GM_Name" character varying(100) NOT NULL,
    "GM_CreateTimeUtc" timestamp with time zone NOT NULL,
    "GM_StartedAtUtc" timestamp with time zone,
    "GM_FinishedAtUtc" timestamp with time zone,
    "GM_Private" boolean NOT NULL,
    "GM_MaxPlayers" integer NOT NULL,
    "GM_WaitForAll" boolean NOT NULL,
    "GM_CreatedByName" text NOT NULL,
    "GM_GM_NextGame_FK" uuid,
    "GM_WinningSubmissionSBM_PK" uuid,
    "GM_QST_FK" uuid NOT NULL,
    CONSTRAINT "PK_Games" PRIMARY KEY ("GM_PK"),
    CONSTRAINT "FK_Games_Games_GM_GM_NextGame_FK" FOREIGN KEY ("GM_GM_NextGame_FK") REFERENCES "Games" ("GM_PK"),
    CONSTRAINT "FK_Games_Questions_GM_QST_FK" FOREIGN KEY ("GM_QST_FK") REFERENCES "Questions" ("QST_PK"),
    CONSTRAINT "FK_Games_Submissions_GM_WinningSubmissionSBM_PK" FOREIGN KEY ("GM_WinningSubmissionSBM_PK") REFERENCES "Submissions" ("SBM_PK")
);

CREATE TABLE "TestRuns" (
    "TCR_PK" uuid NOT NULL,
    "TCR_Status" integer NOT NULL,
    "TCR_ActualResult" text NOT NULL,
    "TCR_Exception" text,
    "TCR_ParentTST_PK" uuid NOT NULL,
    "TCR_SubmissionResultSBM_PK" uuid NOT NULL,
    CONSTRAINT "PK_TestRuns" PRIMARY KEY ("TCR_PK"),
    CONSTRAINT "FK_TestRuns_Submissions_TCR_SubmissionResultSBM_PK" FOREIGN KEY ("TCR_SubmissionResultSBM_PK") REFERENCES "Submissions" ("SBM_PK") ON DELETE CASCADE,
    CONSTRAINT "FK_TestRuns_TestCases_TCR_ParentTST_PK" FOREIGN KEY ("TCR_ParentTST_PK") REFERENCES "TestCases" ("TST_PK") ON DELETE CASCADE
);

CREATE INDEX "IX_GamePlayers_GMP_GM_FK" ON "GamePlayers" ("GMP_GM_FK");

CREATE INDEX "IX_GamePlayers_GMP_USR_FK" ON "GamePlayers" ("GMP_USR_FK");

CREATE INDEX "IX_Games_GM_GM_NextGame_FK" ON "Games" ("GM_GM_NextGame_FK");

CREATE INDEX "IX_Games_GM_QST_FK" ON "Games" ("GM_QST_FK");

CREATE INDEX "IX_Games_GM_WinningSubmissionSBM_PK" ON "Games" ("GM_WinningSubmissionSBM_PK");

CREATE INDEX "IX_Parameters_TC_ScaffoldEXE_PK" ON "Parameters" ("TC_ScaffoldEXE_PK");

CREATE INDEX "IX_Parameters_TC_TypeOT_PK" ON "Parameters" ("TC_TypeOT_PK");

CREATE INDEX "IX_Questions_QST_ScaffoldEXE_PK" ON "Questions" ("QST_ScaffoldEXE_PK");

CREATE INDEX "IX_Scaffolds_EXE_ReturnTypeOT_PK" ON "Scaffolds" ("EXE_ReturnTypeOT_PK");

CREATE INDEX "IX_Submissions_SBM_QuestionQST_PK" ON "Submissions" ("SBM_QuestionQST_PK");

CREATE INDEX "IX_Submissions_SBM_SubmittedByGMP_PK" ON "Submissions" ("SBM_SubmittedByGMP_PK");

CREATE INDEX "IX_TestCases_TST_QST_FK" ON "TestCases" ("TST_QST_FK");

CREATE INDEX "IX_TestRuns_TCR_ParentTST_PK" ON "TestRuns" ("TCR_ParentTST_PK");

CREATE INDEX "IX_TestRuns_TCR_SubmissionResultSBM_PK" ON "TestRuns" ("TCR_SubmissionResultSBM_PK");

CREATE INDEX "IX_Users_USR_GMP_FK" ON "Users" ("USR_GMP_FK");

ALTER TABLE "GamePlayers" ADD CONSTRAINT "FK_GamePlayers_Games_GMP_GM_FK" FOREIGN KEY ("GMP_GM_FK") REFERENCES "Games" ("GM_PK");

ALTER TABLE "GamePlayers" ADD CONSTRAINT "FK_GamePlayers_Users_GMP_USR_FK" FOREIGN KEY ("GMP_USR_FK") REFERENCES "Users" ("USR_PK");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250216012103_Initial-Create', '8.0.7');

COMMIT;


