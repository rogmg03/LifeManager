using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddScheduleBlocks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScheduleBlocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    BlockType = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Status = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: true),
                    TaskId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleBlocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleBlocks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ScheduleBlocks_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ScheduleBlocks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FreeTimeTransactions_ScheduleBlockId",
                table: "FreeTimeTransactions",
                column: "ScheduleBlockId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleBlocks_ProjectId",
                table: "ScheduleBlocks",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleBlocks_TaskId",
                table: "ScheduleBlocks",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleBlocks_UserId_StartTime",
                table: "ScheduleBlocks",
                columns: new[] { "UserId", "StartTime" });

            migrationBuilder.AddForeignKey(
                name: "FK_FreeTimeTransactions_ScheduleBlocks_ScheduleBlockId",
                table: "FreeTimeTransactions",
                column: "ScheduleBlockId",
                principalTable: "ScheduleBlocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FreeTimeTransactions_ScheduleBlocks_ScheduleBlockId",
                table: "FreeTimeTransactions");

            migrationBuilder.DropTable(
                name: "ScheduleBlocks");

            migrationBuilder.DropIndex(
                name: "IX_FreeTimeTransactions_ScheduleBlockId",
                table: "FreeTimeTransactions");
        }
    }
}
