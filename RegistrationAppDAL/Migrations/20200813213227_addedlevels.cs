using Microsoft.EntityFrameworkCore.Migrations;

namespace RegistrationAppDAL.Migrations
{
    public partial class addedlevels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Levels",
                table: "WeeklyPairings",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Levels",
                table: "WeeklyPairings");
        }
    }
}
