using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtualTrainer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BodyGroup",
                columns: table => new
                {
                    IDBodyGroup = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyGroup", x => x.IDBodyGroup);
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    IDEquipment = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.IDEquipment);
                });

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    IDExercise = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Sets = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((3))"),
                    Reps = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((12))"),
                    Weight = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.IDExercise);
                });

            migrationBuilder.CreateTable(
                name: "ProgramType",
                columns: table => new
                {
                    IDProgramType = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProgramTypeName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramType", x => x.IDProgramType);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    IDSubscription = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AllowedTimeInterval = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.IDSubscription);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    IDUser = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<int>(type: "int", nullable: false),
                    CNP = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.IDUser);
                });

            migrationBuilder.CreateTable(
                name: "WorkProgram",
                columns: table => new
                {
                    IDWorkProgram = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProgramName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkProgram", x => x.IDWorkProgram);
                });

            migrationBuilder.CreateTable(
                name: "Group_Equipment",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDBodyGroup = table.Column<int>(type: "int", nullable: false),
                    IDEquipment = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group_Equipment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Group_Equipment_BodyGroup",
                        column: x => x.IDBodyGroup,
                        principalTable: "BodyGroup",
                        principalColumn: "IDBodyGroup",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Group_Equipment_Equipment",
                        column: x => x.IDEquipment,
                        principalTable: "Equipment",
                        principalColumn: "IDEquipment",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Equipment_Exercises",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDEquipment = table.Column<int>(type: "int", nullable: false),
                    IDExercise = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment_Exercises", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Equipment_Exercises_Equipment",
                        column: x => x.IDEquipment,
                        principalTable: "Equipment",
                        principalColumn: "IDEquipment",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Equipment_Exercises_Exercises",
                        column: x => x.IDExercise,
                        principalTable: "Exercises",
                        principalColumn: "IDExercise",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Program_Exercise",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDWorkProgram = table.Column<int>(type: "int", nullable: false),
                    IDExercise = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Program_Exercise", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Program_Exercise_Exercises",
                        column: x => x.IDExercise,
                        principalTable: "Exercises",
                        principalColumn: "IDExercise",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Type_Group",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDBodyGroup = table.Column<int>(type: "int", nullable: false),
                    IDProgramType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Type_Group", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Type_Group_BodyGroup",
                        column: x => x.IDBodyGroup,
                        principalTable: "BodyGroup",
                        principalColumn: "IDBodyGroup",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Type_Group_ProgramType",
                        column: x => x.IDProgramType,
                        principalTable: "ProgramType",
                        principalColumn: "IDProgramType",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User_Subscription",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDUser = table.Column<int>(type: "int", nullable: false),
                    IDSubscription = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    EndDate = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Subscription", x => x.ID);
                    table.ForeignKey(
                        name: "FK_User_Subscription_Subscriptions",
                        column: x => x.IDSubscription,
                        principalTable: "Subscriptions",
                        principalColumn: "IDSubscription",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Subscription_Users",
                        column: x => x.IDUser,
                        principalTable: "Users",
                        principalColumn: "IDUser",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users_Exercises",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDUser = table.Column<int>(type: "int", nullable: false),
                    IDExercise = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users_Exercises", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Users_Exercises_Exercises",
                        column: x => x.IDExercise,
                        principalTable: "Exercises",
                        principalColumn: "IDExercise",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Exercises_Users",
                        column: x => x.IDUser,
                        principalTable: "Users",
                        principalColumn: "IDUser",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Program_Users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDWorkProgram = table.Column<int>(type: "int", nullable: false),
                    IDUser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Program_Users", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Program_Users_Users",
                        column: x => x.IDUser,
                        principalTable: "Users",
                        principalColumn: "IDUser",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Program_Users_WorkProgram",
                        column: x => x.IDWorkProgram,
                        principalTable: "WorkProgram",
                        principalColumn: "IDWorkProgram",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Work_Type",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDWorkProgram = table.Column<int>(type: "int", nullable: false),
                    IDProgramType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Work_Type", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Work_Type_ProgramType",
                        column: x => x.IDProgramType,
                        principalTable: "ProgramType",
                        principalColumn: "IDProgramType",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Work_Type_WorkProgram",
                        column: x => x.IDWorkProgram,
                        principalTable: "WorkProgram",
                        principalColumn: "IDWorkProgram",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_Exercises_IDEquipment",
                table: "Equipment_Exercises",
                column: "IDEquipment");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_Exercises_IDExercise",
                table: "Equipment_Exercises",
                column: "IDExercise");

            migrationBuilder.CreateIndex(
                name: "IX_Group_Equipment_IDBodyGroup",
                table: "Group_Equipment",
                column: "IDBodyGroup");

            migrationBuilder.CreateIndex(
                name: "IX_Group_Equipment_IDEquipment",
                table: "Group_Equipment",
                column: "IDEquipment");

            migrationBuilder.CreateIndex(
                name: "IX_Program_Exercise_IDExercise",
                table: "Program_Exercise",
                column: "IDExercise");

            migrationBuilder.CreateIndex(
                name: "IX_Program_Users_IDUser",
                table: "Program_Users",
                column: "IDUser");

            migrationBuilder.CreateIndex(
                name: "IX_Program_Users_IDWorkProgram",
                table: "Program_Users",
                column: "IDWorkProgram");

            migrationBuilder.CreateIndex(
                name: "IX_Type_Group_IDBodyGroup",
                table: "Type_Group",
                column: "IDBodyGroup");

            migrationBuilder.CreateIndex(
                name: "IX_Type_Group_IDProgramType",
                table: "Type_Group",
                column: "IDProgramType");

            migrationBuilder.CreateIndex(
                name: "IX_User_Subscription_IDSubscription",
                table: "User_Subscription",
                column: "IDSubscription");

            migrationBuilder.CreateIndex(
                name: "IX_User_Subscription_IDUser",
                table: "User_Subscription",
                column: "IDUser");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Exercises_IDExercise",
                table: "Users_Exercises",
                column: "IDExercise");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Exercises_IDUser",
                table: "Users_Exercises",
                column: "IDUser");

            migrationBuilder.CreateIndex(
                name: "IX_Work_Type_IDProgramType",
                table: "Work_Type",
                column: "IDProgramType");

            migrationBuilder.CreateIndex(
                name: "IX_Work_Type_IDWorkProgram",
                table: "Work_Type",
                column: "IDWorkProgram");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Equipment_Exercises");

            migrationBuilder.DropTable(
                name: "Group_Equipment");

            migrationBuilder.DropTable(
                name: "Program_Exercise");

            migrationBuilder.DropTable(
                name: "Program_Users");

            migrationBuilder.DropTable(
                name: "Type_Group");

            migrationBuilder.DropTable(
                name: "User_Subscription");

            migrationBuilder.DropTable(
                name: "Users_Exercises");

            migrationBuilder.DropTable(
                name: "Work_Type");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "BodyGroup");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ProgramType");

            migrationBuilder.DropTable(
                name: "WorkProgram");
        }
    }
}
