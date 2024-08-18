﻿// <auto-generated />
using System;
using CodeTogether.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CodeTogether.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
<<<<<<<< HEAD:CodeTogether.Data/Migrations/20240818070616_Initial-Create.Designer.cs
    [Migration("20240818070616_Initial-Create")]
========
    [Migration("20240818072040_Initial-Create")]
>>>>>>>> d964b19f1340cae6d11d6c39238fe73932fc2d28:CodeTogether.Data/Migrations/20240818072040_Initial-Create.Designer.cs
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CodeTogether.Data.Models.Game.GamePlayerModel", b =>
                {
                    b.Property<Guid>("GMP_PK")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GMP_GM_FK")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GMP_USR_FK")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("GMP_PK");

                    b.HasIndex("GMP_GM_FK");

                    b.HasIndex("GMP_USR_FK");

                    b.ToTable("GamePlayers");
                });

            modelBuilder.Entity("CodeTogether.Data.Models.Game.UserModel", b =>
                {
                    b.Property<Guid>("USR_PK")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("USR_CheckPoints")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("USR_Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid?>("USR_GMP_FK")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("USR_LastHeardFromAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("USR_PasswordHash")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("USR_PasswordSalt")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("USR_UserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("USR_PK");

                    b.HasIndex("USR_GMP_FK");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CodeTogether.Data.Models.Questions.GameModel", b =>
                {
                    b.Property<Guid>("GM_PK")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("GM_CreateTimeUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("GM_CreatedByName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("GM_FinishedAtUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("GM_GM_NextGame_FK")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("GM_MaxPlayers")
                        .HasColumnType("int");

                    b.Property<string>("GM_Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("GM_Private")
                        .HasColumnType("bit");

                    b.Property<Guid>("GM_QST_FK")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("GM_StartedAtUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("GM_USR_FKWinner")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("GM_WaitForAll")
                        .HasColumnType("bit");

<<<<<<<< HEAD:CodeTogether.Data/Migrations/20240818070616_Initial-Create.Designer.cs
                    b.Property<Guid?>("GM_WinningSubmissionSBM_PK")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("GM_PK");

                    b.HasIndex("GM_GM_NextGame_FK");

                    b.HasIndex("GM_QST_FK");

                    b.HasIndex("GM_WinningSubmissionSBM_PK");
========
                    b.Property<string>("GM_WinnerCode")
                        .HasMaxLength(2147483647)
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("GM_PK");

                    b.HasIndex("GM_USR_FKWinner");
>>>>>>>> d964b19f1340cae6d11d6c39238fe73932fc2d28:CodeTogether.Data/Migrations/20240818072040_Initial-Create.Designer.cs

                    b.ToTable("Games");
                });

            modelBuilder.Entity("CodeTogether.Data.Models.Questions.ParameterModel", b =>
                {
                    b.Property<Guid>("TC_PK")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TC_Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("TC_Position")
                        .HasColumnType("int");

                    b.Property<Guid>("TC_ScaffoldEXE_PK")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TC_TypeOT_PK")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TC_PK");

                    b.HasIndex("TC_ScaffoldEXE_PK");

                    b.HasIndex("TC_TypeOT_PK");

                    b.ToTable("Parameters");
                });

            modelBuilder.Entity("CodeTogether.Data.Models.Questions.QuestionModel", b =>
                {
                    b.Property<Guid>("QST_PK")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("QST_Description")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("QST_Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<Guid>("QST_ScaffoldEXE_PK")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("QST_PK");

                    b.HasIndex("QST_ScaffoldEXE_PK");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("CodeTogether.Data.Models.Questions.ScaffoldModel", b =>
                {
                    b.Property<Guid>("EXE_PK")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EXE_ExecutionRunnerArgument")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("EXE_ExecutionRunnerName")
                        .HasMaxLength(50)
                        .HasColumnType("int");

                    b.Property<Guid>("EXE_ReturnTypeOT_PK")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EXE_ScaffoldName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("EXE_ScaffoldText")
                        .IsRequired()
                        .HasMaxLength(2147483647)
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EXE_PK");

                    b.HasIndex("EXE_ReturnTypeOT_PK");

                    b.ToTable("Scaffolds");
                });

            modelBuilder.Entity("CodeTogether.Data.Models.Questions.TestCaseModel", b =>
                {
                    b.Property<Guid>("TST_PK")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TST_Arguments")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("TST_ExpectedResponse")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("TST_IsHidden")
                        .HasColumnType("bit");

                    b.Property<Guid>("TST_QST_FK")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TST_Title")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("TST_PK");

                    b.HasIndex("TST_QST_FK");

                    b.ToTable("TestCases");
                });

            modelBuilder.Entity("CodeTogether.Data.Models.Questions.TestRunModel", b =>
                {
                    b.Property<Guid>("TCR_PK")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TCR_ActualResult")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("TCR_Exception")
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.Property<Guid>("TCR_ParentTST_PK")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("TCR_Status")
                        .HasColumnType("int");

                    b.Property<Guid>("TCR_SubmissionResultSBM_PK")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TCR_PK");

                    b.HasIndex("TCR_ParentTST_PK");

                    b.HasIndex("TCR_SubmissionResultSBM_PK");

                    b.ToTable("TestRuns");
                });

            modelBuilder.Entity("CodeTogether.Data.Models.Questions.TypeModel", b =>
                {
                    b.Property<Guid>("OT_PK")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("OT_AssemblyName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("OT_TypeName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("OT_PK");

                    b.ToTable("Types");
                });

            modelBuilder.Entity("CodeTogether.Data.Models.StmDataModel", b =>
                {
                    b.Property<Guid>("STM_PK")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("STM_Key")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("STM_Value")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("STM_PK");

                    b.ToTable("StmData");
                });

            modelBuilder.Entity("CodeTogether.Data.Models.Submission.SubmissionModel", b =>
                {
                    b.Property<Guid>("SBM_PK")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SBM_Code")
                        .IsRequired()
                        .HasMaxLength(2147483647)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SBM_CompileError")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SBM_QuestionQST_PK")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("SBM_Status")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("SBM_SubmissionDuration")
                        .HasColumnType("time");

                    b.Property<DateTime>("SBM_SubmissionStartTimeUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("SBM_SubmittedByGMP_PK")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("SBM_PK");

                    b.HasIndex("SBM_QuestionQST_PK");

                    b.HasIndex("SBM_SubmittedByGMP_PK");

                    b.ToTable("Submissions");
                });

            modelBuilder.Entity("CodeTogether.Data.Models.Game.GamePlayerModel", b =>
                {
                    b.HasOne("CodeTogether.Data.Models.Questions.GameModel", "GMP_Game")
                        .WithMany("GamePlayers")
                        .HasForeignKey("GMP_GM_FK")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CodeTogether.Data.Models.Game.UserModel", "GMP_User")
                        .WithMany("GamePlayers")
                        .HasForeignKey("GMP_USR_FK")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("GMP_Game");

                    b.Navigation("GMP_User");
                });

            modelBuilder.Entity("CodeTogether.Data.Models.Game.UserModel", b =>
                {
                    b.HasOne("CodeTogether.Data.Models.Game.GamePlayerModel", "USR_CurrentGame")
                        .WithMany()
                        .HasForeignKey("USR_GMP_FK");

                    b.Navigation("USR_CurrentGame");
                });

            modelBuilder.Entity("CodeTogether.Data.Models.Questions.GameModel", b =>
                {
<<<<<<<< HEAD:CodeTogether.Data/Migrations/20240818070616_Initial-Create.Designer.cs
                    b.HasOne("CodeTogether.Data.Models.Questions.GameModel", "GM_NextGame")
                        .WithMany()
                        .HasForeignKey("GM_GM_NextGame_FK")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("CodeTogether.Data.Models.Questions.QuestionModel", "GM_Question")
                        .WithMany()
                        .HasForeignKey("GM_QST_FK")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CodeTogether.Data.Models.Submission.SubmissionModel", "GM_WinningSubmission")
                        .WithMany()
                        .HasForeignKey("GM_WinningSubmissionSBM_PK")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("GM_NextGame");

                    b.Navigation("GM_Question");

                    b.Navigation("GM_WinningSubmission");
========
                    b.HasOne("CodeTogether.Data.Models.Game.UserModel", "GM_Winner")
                        .WithMany()
                        .HasForeignKey("GM_USR_FKWinner");

                    b.Navigation("GM_Winner");
>>>>>>>> d964b19f1340cae6d11d6c39238fe73932fc2d28:CodeTogether.Data/Migrations/20240818072040_Initial-Create.Designer.cs
                });

            modelBuilder.Entity("CodeTogether.Data.Models.Questions.ParameterModel", b =>
                {
                    b.HasOne("CodeTogether.Data.Models.Questions.ScaffoldModel", "TC_Scaffold")
                        .WithMany("EXE_Parameters")
                        .HasForeignKey("TC_ScaffoldEXE_PK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CodeTogether.Data.Models.Questions.TypeModel", "TC_Type")
                        .WithMany()
                        .HasForeignKey("TC_TypeOT_PK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TC_Scaffold");

                    b.Navigation("TC_Type");
                });

            modelBuilder.Entity("CodeTogether.Data.Models.Questions.QuestionModel", b =>
                {
                    b.HasOne("CodeTogether.Data.Models.Questions.ScaffoldModel", "QST_Scaffold")
                        .WithMany()
                        .HasForeignKey("QST_ScaffoldEXE_PK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("QST_Scaffold");
                });

            modelBuilder.Entity("CodeTogether.Data.Models.Questions.ScaffoldModel", b =>
                {
                    b.HasOne("CodeTogether.Data.Models.Questions.TypeModel", "EXE_ReturnType")
                        .WithMany()
                        .HasForeignKey("EXE_ReturnTypeOT_PK")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("EXE_ReturnType");
                });

            modelBuilder.Entity("CodeTogether.Data.Models.Questions.TestCaseModel", b =>
                {
                    b.HasOne("CodeTogether.Data.Models.Questions.QuestionModel", "TST_Question")
                        .WithMany("QST_TestCases")
                        .HasForeignKey("TST_QST_FK")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("TST_Question");
                });

            modelBuilder.Entity("CodeTogether.Data.Models.Questions.TestRunModel", b =>
                {
                    b.HasOne("CodeTogether.Data.Models.Questions.TestCaseModel", "TCR_Parent")
                        .WithMany()
                        .HasForeignKey("TCR_ParentTST_PK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CodeTogether.Data.Models.Submission.SubmissionModel", "TCR_SubmissionResult")
                        .WithMany("SBM_TestRuns")
                        .HasForeignKey("TCR_SubmissionResultSBM_PK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TCR_Parent");

                    b.Navigation("TCR_SubmissionResult");
                });

            modelBuilder.Entity("CodeTogether.Data.Models.Submission.SubmissionModel", b =>
                {
                    b.HasOne("CodeTogether.Data.Models.Questions.QuestionModel", "SBM_Question")
                        .WithMany()
                        .HasForeignKey("SBM_QuestionQST_PK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CodeTogether.Data.Models.Game.GamePlayerModel", "SBM_SubmittedBy")
                        .WithMany("Submissions")
                        .HasForeignKey("SBM_SubmittedByGMP_PK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SBM_Question");

                    b.Navigation("SBM_SubmittedBy");
                });

            modelBuilder.Entity("CodeTogether.Data.Models.Game.GamePlayerModel", b =>
                {
                    b.Navigation("Submissions");
                });

            modelBuilder.Entity("CodeTogether.Data.Models.Game.UserModel", b =>
                {
                    b.Navigation("GamePlayers");
                });

            modelBuilder.Entity("CodeTogether.Data.Models.Questions.GameModel", b =>
                {
                    b.Navigation("GamePlayers");
                });

            modelBuilder.Entity("CodeTogether.Data.Models.Questions.QuestionModel", b =>
                {
                    b.Navigation("QST_TestCases");
                });

            modelBuilder.Entity("CodeTogether.Data.Models.Questions.ScaffoldModel", b =>
                {
                    b.Navigation("EXE_Parameters");
                });

            modelBuilder.Entity("CodeTogether.Data.Models.Submission.SubmissionModel", b =>
                {
                    b.Navigation("SBM_TestRuns");
                });
#pragma warning restore 612, 618
        }
    }
}
