using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeartSpace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addconfigforpayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentRequest_Appointments_AppointmentId",
                table: "PaymentRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentRequest_Users_ConsultantId",
                table: "PaymentRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentRequest",
                table: "PaymentRequest");

            migrationBuilder.RenameTable(
                name: "PaymentRequest",
                newName: "PaymentRequests");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentRequest_ConsultantId",
                table: "PaymentRequests",
                newName: "IX_PaymentRequests_ConsultantId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentRequest_AppointmentId",
                table: "PaymentRequests",
                newName: "IX_PaymentRequests_AppointmentId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "PaymentRequests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ProcessedAt",
                table: "PaymentRequests",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BankName",
                table: "PaymentRequests",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "BankAccount",
                table: "PaymentRequests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentRequests",
                table: "PaymentRequests",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentRequests_Appointments_AppointmentId",
                table: "PaymentRequests",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentRequests_Users_ConsultantId",
                table: "PaymentRequests",
                column: "ConsultantId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentRequests_Appointments_AppointmentId",
                table: "PaymentRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentRequests_Users_ConsultantId",
                table: "PaymentRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentRequests",
                table: "PaymentRequests");

            migrationBuilder.RenameTable(
                name: "PaymentRequests",
                newName: "PaymentRequest");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentRequests_ConsultantId",
                table: "PaymentRequest",
                newName: "IX_PaymentRequest_ConsultantId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentRequests_AppointmentId",
                table: "PaymentRequest",
                newName: "IX_PaymentRequest_AppointmentId");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "PaymentRequest",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ProcessedAt",
                table: "PaymentRequest",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<string>(
                name: "BankName",
                table: "PaymentRequest",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "BankAccount",
                table: "PaymentRequest",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentRequest",
                table: "PaymentRequest",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentRequest_Appointments_AppointmentId",
                table: "PaymentRequest",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentRequest_Users_ConsultantId",
                table: "PaymentRequest",
                column: "ConsultantId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
