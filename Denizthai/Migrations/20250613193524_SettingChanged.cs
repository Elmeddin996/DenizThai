using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Denizthai.Migrations
{
    public partial class SettingChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AboutAz",
                table: "Settings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AboutEn",
                table: "Settings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AboutRu",
                table: "Settings",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AboutAz",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "AboutEn",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "AboutRu",
                table: "Settings");
        }
    }
}
