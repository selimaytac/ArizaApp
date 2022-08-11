using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ArizaApp.Migrations
{
    public partial class ManytoManyRelationAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "ArizaModels",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 8, 11, 15, 20, 15, 622, DateTimeKind.Local).AddTicks(1427),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 8, 9, 10, 18, 44, 58, DateTimeKind.Local).AddTicks(1072));

            migrationBuilder.CreateTable(
                name: "ArizaModelFirmRecord",
                columns: table => new
                {
                    ArizaModelsId = table.Column<int>(type: "int", nullable: false),
                    FirmsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArizaModelFirmRecord", x => new { x.ArizaModelsId, x.FirmsId });
                    table.ForeignKey(
                        name: "FK_ArizaModelFirmRecord_ArizaModels_ArizaModelsId",
                        column: x => x.ArizaModelsId,
                        principalTable: "ArizaModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArizaModelFirmRecord_FirmRecords_FirmsId",
                        column: x => x.FirmsId,
                        principalTable: "FirmRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArizaModelFirmRecord_FirmsId",
                table: "ArizaModelFirmRecord",
                column: "FirmsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArizaModelFirmRecord");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "ArizaModels",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 8, 9, 10, 18, 44, 58, DateTimeKind.Local).AddTicks(1072),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 8, 11, 15, 20, 15, 622, DateTimeKind.Local).AddTicks(1427));
        }
    }
}
