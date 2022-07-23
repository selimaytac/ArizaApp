using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ArizaApp.Migrations
{
    public partial class EmailsDescriptionAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "EmailRecords");

            migrationBuilder.AddColumn<string>(
                name: "EmailDescription",
                table: "EmailRecords",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "ArizaModels",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 9, 22, 22, 47, 367, DateTimeKind.Local).AddTicks(5175),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 7, 9, 21, 39, 45, 281, DateTimeKind.Local).AddTicks(1811));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailDescription",
                table: "EmailRecords");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "EmailRecords",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "ArizaModels",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 9, 21, 39, 45, 281, DateTimeKind.Local).AddTicks(1811),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 7, 9, 22, 22, 47, 367, DateTimeKind.Local).AddTicks(5175));
        }
    }
}
