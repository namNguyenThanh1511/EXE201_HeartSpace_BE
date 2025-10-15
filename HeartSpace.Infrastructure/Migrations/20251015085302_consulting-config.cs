using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeartSpace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class consultingconfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bio",
                table: "ConsultantProfiles");

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "HourlyRate",
                table: "ConsultantProfiles",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateTable(
                name: "Consultings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreateAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConsultantConsulting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConsultingId = table.Column<int>(type: "int", nullable: false),
                    ConsultantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultantConsulting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsultantConsulting_Consultings_ConsultingId",
                        column: x => x.ConsultingId,
                        principalTable: "Consultings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsultantConsulting_Users_ConsultantId",
                        column: x => x.ConsultantId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsultantConsulting_ConsultantId",
                table: "ConsultantConsulting",
                column: "ConsultantId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultantConsulting_ConsultingId",
                table: "ConsultantConsulting",
                column: "ConsultingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsultantConsulting");

            migrationBuilder.DropTable(
                name: "Consultings");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "Users");

            migrationBuilder.AlterColumn<decimal>(
                name: "HourlyRate",
                table: "ConsultantProfiles",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "ConsultantProfiles",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");
        }
    }
}
