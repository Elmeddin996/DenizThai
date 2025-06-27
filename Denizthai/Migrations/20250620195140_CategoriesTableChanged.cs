using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Denizthai.Migrations
{
    public partial class CategoriesTableChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Categorias",
                table: "Categorias");

            migrationBuilder.RenameTable(
                name: "Categorias",
                newName: "Categories");

            migrationBuilder.RenameColumn(
                name: "NameAn",
                table: "Tours",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "NameAn",
                table: "Categories",
                newName: "NameAz");

            migrationBuilder.AddColumn<int>(
                name: "CategorieId",
                table: "Tours",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAz",
                table: "Tours",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "Tours",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionRu",
                table: "Tours",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DiscountedPrice",
                table: "Tours",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Tours",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LocationAz",
                table: "Tours",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LocationEn",
                table: "Tours",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LocationRu",
                table: "Tours",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameAz",
                table: "Tours",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tours_CategorieId",
                table: "Tours",
                column: "CategorieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tours_Categories_CategorieId",
                table: "Tours",
                column: "CategorieId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tours_Categories_CategorieId",
                table: "Tours");

            migrationBuilder.DropIndex(
                name: "IX_Tours_CategorieId",
                table: "Tours");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CategorieId",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "DescriptionAz",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "DescriptionRu",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "DiscountedPrice",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "LocationAz",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "LocationEn",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "LocationRu",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "NameAz",
                table: "Tours");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Categorias");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Tours",
                newName: "NameAn");

            migrationBuilder.RenameColumn(
                name: "NameAz",
                table: "Categorias",
                newName: "NameAn");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categorias",
                table: "Categorias",
                column: "Id");
        }
    }
}
