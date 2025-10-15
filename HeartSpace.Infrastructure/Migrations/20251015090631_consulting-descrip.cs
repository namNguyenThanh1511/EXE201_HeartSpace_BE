using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeartSpace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class consultingdescrip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Consultings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Consultings");
        }
    }
}
