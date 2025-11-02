using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeartSpace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addmeetlinkuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MeetingLink",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeetingLink",
                table: "Users");
        }
    }
}
