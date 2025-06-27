using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Denizthai.Migrations
{
    public partial class InstaLinkAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InstaLink",
                table: "Settings",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstaLink",
                table: "Settings");
        }
    }
}
