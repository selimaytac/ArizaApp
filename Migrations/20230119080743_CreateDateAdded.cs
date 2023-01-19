using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ArizaApp.Migrations
{
    public partial class CreateDateAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UploadDate",
                table: "UploadedFiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 19, 11, 7, 42, 620, DateTimeKind.Local).AddTicks(4940),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 11, 28, 15, 3, 4, 28, DateTimeKind.Local).AddTicks(405));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ArizaModels",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 19, 11, 7, 42, 604, DateTimeKind.Local).AddTicks(9285));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ArizaModels");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UploadDate",
                table: "UploadedFiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 28, 15, 3, 4, 28, DateTimeKind.Local).AddTicks(405),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 1, 19, 11, 7, 42, 620, DateTimeKind.Local).AddTicks(4940));
        }
    }
}
