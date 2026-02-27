using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOnlineCourseDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OnlineCourseDetails",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Platform = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    CourseUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    InstructorName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    TotalLessons = table.Column<int>(type: "integer", nullable: true),
                    CompletedLessons = table.Column<int>(type: "integer", nullable: true),
                    CertificateUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    StartedAt = table.Column<DateOnly>(type: "date", nullable: true),
                    CompletedAt = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlineCourseDetails", x => x.ProjectId);
                    table.ForeignKey(
                        name: "FK_OnlineCourseDetails_Projects_ProjectId",
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
                name: "OnlineCourseDetails");
        }
    }
}
