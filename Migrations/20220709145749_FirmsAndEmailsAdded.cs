using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ArizaApp.Migrations
{
    public partial class FirmsAndEmailsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EmailAddress",
                table: "EmailRecords",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "ArizaModels",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 9, 17, 57, 48, 742, DateTimeKind.Local).AddTicks(8823),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 7, 9, 17, 56, 40, 265, DateTimeKind.Local).AddTicks(7232));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EmailAddress",
                table: "EmailRecords",
                type: "int",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "ArizaModels",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 9, 17, 56, 40, 265, DateTimeKind.Local).AddTicks(7232),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 7, 9, 17, 57, 48, 742, DateTimeKind.Local).AddTicks(8823));
        }
    }
}
