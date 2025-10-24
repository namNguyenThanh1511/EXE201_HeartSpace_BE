using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeartSpace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixtablename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Schedule_ScheduleId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Users_ConsultantId",
                table: "Schedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schedule",
                table: "Schedule");

            migrationBuilder.RenameTable(
                name: "Schedule",
                newName: "Schedules");

            migrationBuilder.RenameIndex(
                name: "IX_Schedule_ConsultantId",
                table: "Schedules",
                newName: "IX_Schedules_ConsultantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Schedules_ScheduleId",
                table: "Appointments",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Users_ConsultantId",
                table: "Schedules",
                column: "ConsultantId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Schedules_ScheduleId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Users_ConsultantId",
                table: "Schedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules");

            migrationBuilder.RenameTable(
                name: "Schedules",
                newName: "Schedule");

            migrationBuilder.RenameIndex(
                name: "IX_Schedules_ConsultantId",
                table: "Schedule",
                newName: "IX_Schedule_ConsultantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schedule",
                table: "Schedule",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Schedule_ScheduleId",
                table: "Appointments",
                column: "ScheduleId",
                principalTable: "Schedule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Users_ConsultantId",
                table: "Schedule",
                column: "ConsultantId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
