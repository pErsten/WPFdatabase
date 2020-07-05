using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectSTSlore.Migrations
{
    public partial class FirstMigration : Migration
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
                    image = table.Column<string>(nullable: true)
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
                name: "Subject",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    subjectName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.id);
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
                name: "Teacher",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    personid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teacher", x => x.id);
                    table.ForeignKey(
                        name: "FK_Teacher_Persons_personid",
                        column: x => x.personid,
                        principalTable: "Persons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teacher_Subject",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    subjectid = table.Column<int>(nullable: true),
                    teacherid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teacher_Subject", x => x.id);
                    table.ForeignKey(
                        name: "FK_Teacher_Subject_Subject_subjectid",
                        column: x => x.subjectid,
                        principalTable: "Subject",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Teacher_Subject_Teacher_teacherid",
                        column: x => x.teacherid,
                        principalTable: "Teacher",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Group_TeacherSubject",
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
                    table.PrimaryKey("PK_Group_TeacherSubject", x => x.id);
                    table.ForeignKey(
                        name: "FK_Group_TeacherSubject_Groups_groupid",
                        column: x => x.groupid,
                        principalTable: "Groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Group_TeacherSubject_Teacher_Subject_teacherSubjectid",
                        column: x => x.teacherSubjectid,
                        principalTable: "Teacher_Subject",
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
                        name: "FK_Marks_Group_TeacherSubject_subjectForMarksid",
                        column: x => x.subjectForMarksid,
                        principalTable: "Group_TeacherSubject",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Group_TeacherSubject_groupid",
                table: "Group_TeacherSubject",
                column: "groupid");

            migrationBuilder.CreateIndex(
                name: "IX_Group_TeacherSubject_teacherSubjectid",
                table: "Group_TeacherSubject",
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
                name: "IX_Teacher_personid",
                table: "Teacher",
                column: "personid");

            migrationBuilder.CreateIndex(
                name: "IX_Teacher_Subject_subjectid",
                table: "Teacher_Subject",
                column: "subjectid");

            migrationBuilder.CreateIndex(
                name: "IX_Teacher_Subject_teacherid",
                table: "Teacher_Subject",
                column: "teacherid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Marks");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Group_TeacherSubject");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Teacher_Subject");

            migrationBuilder.DropTable(
                name: "Subject");

            migrationBuilder.DropTable(
                name: "Teacher");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
