using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFreeTimeTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EarnedTransactionId",
                table: "TimeEntries",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FreeTimeRatios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkMinutesPerFreeMinute = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false, defaultValue: 1.0m),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreeTimeRatios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FreeTimeRatios_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FreeTimeTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TimeEntryId = table.Column<Guid>(type: "uuid", nullable: true),
                    ScheduleBlockId = table.Column<Guid>(type: "uuid", nullable: true),
                    Type = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    MinutesDelta = table.Column<int>(type: "integer", nullable: false),
                    BalanceAfterMinutes = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreeTimeTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FreeTimeTransactions_TimeEntries_TimeEntryId",
                        column: x => x.TimeEntryId,
                        principalTable: "TimeEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_FreeTimeTransactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntries_EarnedTransactionId",
                table: "TimeEntries",
                column: "EarnedTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_FreeTimeRatios_UserId",
                table: "FreeTimeRatios",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FreeTimeTransactions_CreatedAt",
                table: "FreeTimeTransactions",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_FreeTimeTransactions_TimeEntryId",
                table: "FreeTimeTransactions",
                column: "TimeEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_FreeTimeTransactions_UserId",
                table: "FreeTimeTransactions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeEntries_FreeTimeTransactions_EarnedTransactionId",
                table: "TimeEntries",
                column: "EarnedTransactionId",
                principalTable: "FreeTimeTransactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeEntries_FreeTimeTransactions_EarnedTransactionId",
                table: "TimeEntries");

            migrationBuilder.DropTable(
                name: "FreeTimeRatios");

            migrationBuilder.DropTable(
                name: "FreeTimeTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TimeEntries_EarnedTransactionId",
                table: "TimeEntries");

            migrationBuilder.DropColumn(
                name: "EarnedTransactionId",
                table: "TimeEntries");
        }
    }
}
