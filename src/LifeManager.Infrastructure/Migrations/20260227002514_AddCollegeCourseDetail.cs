using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCollegeCourseDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CollegeCourseDetails",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    InstitutionName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CourseName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CourseCode = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Semester = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Year = table.Column<int>(type: "integer", nullable: true),
                    Credits = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    Professor = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Room = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Schedule = table.Column<string>(type: "text", nullable: true),
                    CurrentGrade = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    TargetGrade = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollegeCourseDetails", x => x.ProjectId);
                    table.ForeignKey(
                        name: "FK_CollegeCourseDetails_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollegeCourseDetails");
        }
    }
}
