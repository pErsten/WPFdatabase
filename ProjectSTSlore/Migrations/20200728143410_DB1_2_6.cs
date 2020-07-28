using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectSTSlore.Migrations
{
    public partial class DB1_2_6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    groupNumber = table.Column<int>(nullable: true),
                    image = table.Column<byte[]>(type: "blob", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(nullable: true),
                    surname = table.Column<string>(nullable: true),
                    patronymic = table.Column<string>(nullable: true),
                    address = table.Column<string>(nullable: true),
                    personRole = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    subjectName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    groupid = table.Column<int>(nullable: true),
                    personid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.id);
                    table.ForeignKey(
                        name: "FK_Students_Groups_groupid",
                        column: x => x.groupid,
                        principalTable: "Groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_Persons_personid",
                        column: x => x.personid,
                        principalTable: "Persons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    personid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.id);
                    table.ForeignKey(
                        name: "FK_Teachers_Persons_personid",
                        column: x => x.personid,
                        principalTable: "Persons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teacher_Subjects",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    subjectid = table.Column<int>(nullable: true),
                    teacherid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teacher_Subjects", x => x.id);
                    table.ForeignKey(
                        name: "FK_Teacher_Subjects_Subjects_subjectid",
                        column: x => x.subjectid,
                        principalTable: "Subjects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Teacher_Subjects_Teachers_teacherid",
                        column: x => x.teacherid,
                        principalTable: "Teachers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Group_TeacherSubjects",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    teacherSubjectid = table.Column<int>(nullable: true),
                    groupid = table.Column<int>(nullable: true),
                    hours = table.Column<byte>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group_TeacherSubjects", x => x.id);
                    table.ForeignKey(
                        name: "FK_Group_TeacherSubjects_Groups_groupid",
                        column: x => x.groupid,
                        principalTable: "Groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Group_TeacherSubjects_Teacher_Subjects_teacherSubjectid",
                        column: x => x.teacherSubjectid,
                        principalTable: "Teacher_Subjects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Marks",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    studentid = table.Column<int>(nullable: true),
                    subjectForMarksid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marks", x => x.id);
                    table.ForeignKey(
                        name: "FK_Marks_Students_studentid",
                        column: x => x.studentid,
                        principalTable: "Students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Marks_Group_TeacherSubjects_subjectForMarksid",
                        column: x => x.subjectForMarksid,
                        principalTable: "Group_TeacherSubjects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Group_TeacherSubjects_groupid",
                table: "Group_TeacherSubjects",
                column: "groupid");

            migrationBuilder.CreateIndex(
                name: "IX_Group_TeacherSubjects_teacherSubjectid",
                table: "Group_TeacherSubjects",
                column: "teacherSubjectid");

            migrationBuilder.CreateIndex(
                name: "IX_Marks_studentid",
                table: "Marks",
                column: "studentid");

            migrationBuilder.CreateIndex(
                name: "IX_Marks_subjectForMarksid",
                table: "Marks",
                column: "subjectForMarksid");

            migrationBuilder.CreateIndex(
                name: "IX_Students_groupid",
                table: "Students",
                column: "groupid");

            migrationBuilder.CreateIndex(
                name: "IX_Students_personid",
                table: "Students",
                column: "personid");

            migrationBuilder.CreateIndex(
                name: "IX_Teacher_Subjects_subjectid",
                table: "Teacher_Subjects",
                column: "subjectid");

            migrationBuilder.CreateIndex(
                name: "IX_Teacher_Subjects_teacherid",
                table: "Teacher_Subjects",
                column: "teacherid");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_personid",
                table: "Teachers",
                column: "personid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Marks");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Group_TeacherSubjects");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Teacher_Subjects");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
