using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ArizaApp.Migrations
{
    public partial class EmailsAndFirmsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "ArizaModels",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 9, 21, 39, 45, 281, DateTimeKind.Local).AddTicks(1811),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 7, 9, 17, 41, 17, 469, DateTimeKind.Local).AddTicks(3246));

            migrationBuilder.CreateTable(
                name: "EmailRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FirmRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirmName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FirmRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailRecordFirmRecord",
                columns: table => new
                {
                    EmailsId = table.Column<int>(type: "int", nullable: false),
                    FirmsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailRecordFirmRecord", x => new { x.EmailsId, x.FirmsId });
                    table.ForeignKey(
                        name: "FK_EmailRecordFirmRecord_EmailRecords_EmailsId",
                        column: x => x.EmailsId,
                        principalTable: "EmailRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmailRecordFirmRecord_FirmRecords_FirmsId",
                        column: x => x.FirmsId,
                        principalTable: "FirmRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailRecordFirmRecord_FirmsId",
                table: "EmailRecordFirmRecord",
                column: "FirmsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailRecordFirmRecord");

            migrationBuilder.DropTable(
                name: "EmailRecords");

            migrationBuilder.DropTable(
                name: "FirmRecords");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "ArizaModels",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 9, 17, 41, 17, 469, DateTimeKind.Local).AddTicks(3246),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 7, 9, 21, 39, 45, 281, DateTimeKind.Local).AddTicks(1811));
        }
    }
}
