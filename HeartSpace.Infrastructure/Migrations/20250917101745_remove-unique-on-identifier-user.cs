using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestSetup.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeuniqueonidentifieruser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Identifier",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_Identifier",
                table: "Users",
                column: "Identifier",
                unique: true);
        }
    }
}
