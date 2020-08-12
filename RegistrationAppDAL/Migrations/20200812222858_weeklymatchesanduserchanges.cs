using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RegistrationAppDAL.Migrations
{
    public partial class weeklymatchesanduserchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attending",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "AllPairingsModelId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttendingId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Levels",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Attending",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Levels = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attending", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormerMatch",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PartnerId = table.Column<string>(nullable: false),
                    DateDanced = table.Column<DateTime>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormerMatch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormerMatch_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WeeklyPairings",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyPairings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pairing",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    MaleUserId = table.Column<string>(nullable: false),
                    FemaleUserId = table.Column<string>(nullable: false),
                    AllPairingsModelId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pairing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pairing_WeeklyPairings_AllPairingsModelId",
                        column: x => x.AllPairingsModelId,
                        principalTable: "WeeklyPairings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AllPairingsModelId",
                table: "AspNetUsers",
                column: "AllPairingsModelId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AttendingId",
                table: "AspNetUsers",
                column: "AttendingId");

            migrationBuilder.CreateIndex(
                name: "IX_FormerMatch_ApplicationUserId",
                table: "FormerMatch",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Pairing_AllPairingsModelId",
                table: "Pairing",
                column: "AllPairingsModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_WeeklyPairings_AllPairingsModelId",
                table: "AspNetUsers",
                column: "AllPairingsModelId",
                principalTable: "WeeklyPairings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Attending_AttendingId",
                table: "AspNetUsers",
                column: "AttendingId",
                principalTable: "Attending",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_WeeklyPairings_AllPairingsModelId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Attending_AttendingId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Attending");

            migrationBuilder.DropTable(
                name: "FormerMatch");

            migrationBuilder.DropTable(
                name: "Pairing");

            migrationBuilder.DropTable(
                name: "WeeklyPairings");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AllPairingsModelId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AttendingId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AllPairingsModelId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AttendingId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Levels",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<DateTime>(
                name: "Attending",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
