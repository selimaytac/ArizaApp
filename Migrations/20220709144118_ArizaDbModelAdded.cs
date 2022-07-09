using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ArizaApp.Migrations
{
    public partial class ArizaDbModelAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArizaModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FaultNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FaultType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    State = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2022, 7, 9, 17, 41, 17, 469, DateTimeKind.Local).AddTicks(3246)),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FailureCause = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AlarmStatus = table.Column<bool>(type: "bit", nullable: false),
                    AffectedServices = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AffectedFirms = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SendMail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArizaModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArizaModels_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArizaModels_UserId",
                table: "ArizaModels",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArizaModels");
        }
    }
}
