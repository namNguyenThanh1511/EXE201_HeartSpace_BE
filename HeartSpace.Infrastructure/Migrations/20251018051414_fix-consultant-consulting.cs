using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeartSpace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixconsultantconsulting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsultantConsulting_Consultings_ConsultingId",
                table: "ConsultantConsulting");

            migrationBuilder.DropForeignKey(
                name: "FK_ConsultantConsulting_Users_ConsultantId",
                table: "ConsultantConsulting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConsultantConsulting",
                table: "ConsultantConsulting");

            migrationBuilder.RenameTable(
                name: "ConsultantConsulting",
                newName: "ConsultantConsultings");

            migrationBuilder.RenameIndex(
                name: "IX_ConsultantConsulting_ConsultingId",
                table: "ConsultantConsultings",
                newName: "IX_ConsultantConsultings_ConsultingId");

            migrationBuilder.RenameIndex(
                name: "IX_ConsultantConsulting_ConsultantId",
                table: "ConsultantConsultings",
                newName: "IX_ConsultantConsultings_ConsultantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConsultantConsultings",
                table: "ConsultantConsultings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsultantConsultings_Consultings_ConsultingId",
                table: "ConsultantConsultings",
                column: "ConsultingId",
                principalTable: "Consultings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConsultantConsultings_Users_ConsultantId",
                table: "ConsultantConsultings",
                column: "ConsultantId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsultantConsultings_Consultings_ConsultingId",
                table: "ConsultantConsultings");

            migrationBuilder.DropForeignKey(
                name: "FK_ConsultantConsultings_Users_ConsultantId",
                table: "ConsultantConsultings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConsultantConsultings",
                table: "ConsultantConsultings");

            migrationBuilder.RenameTable(
                name: "ConsultantConsultings",
                newName: "ConsultantConsulting");

            migrationBuilder.RenameIndex(
                name: "IX_ConsultantConsultings_ConsultingId",
                table: "ConsultantConsulting",
                newName: "IX_ConsultantConsulting_ConsultingId");

            migrationBuilder.RenameIndex(
                name: "IX_ConsultantConsultings_ConsultantId",
                table: "ConsultantConsulting",
                newName: "IX_ConsultantConsulting_ConsultantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConsultantConsulting",
                table: "ConsultantConsulting",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsultantConsulting_Consultings_ConsultingId",
                table: "ConsultantConsulting",
                column: "ConsultingId",
                principalTable: "Consultings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConsultantConsulting_Users_ConsultantId",
                table: "ConsultantConsulting",
                column: "ConsultantId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
