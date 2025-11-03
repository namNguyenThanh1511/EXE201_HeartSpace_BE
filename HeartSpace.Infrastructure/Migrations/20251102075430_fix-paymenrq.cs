using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeartSpace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixpaymenrq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ConsultantId",
                table: "PaymentRequest",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRequest_ConsultantId",
                table: "PaymentRequest",
                column: "ConsultantId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentRequest_Users_ConsultantId",
                table: "PaymentRequest",
                column: "ConsultantId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentRequest_Users_ConsultantId",
                table: "PaymentRequest");

            migrationBuilder.DropIndex(
                name: "IX_PaymentRequest_ConsultantId",
                table: "PaymentRequest");

            migrationBuilder.DropColumn(
                name: "ConsultantId",
                table: "PaymentRequest");
        }
    }
}
