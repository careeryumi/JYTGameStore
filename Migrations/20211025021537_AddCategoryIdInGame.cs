using Microsoft.EntityFrameworkCore.Migrations;

namespace JYTGameStore.Migrations
{
    public partial class AddCategoryIdInGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "categoryId",
                table: "Game",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Game_categoryId",
                table: "Game",
                column: "categoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Category_categoryId",
                table: "Game",
                column: "categoryId",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_Category_categoryId",
                table: "Game");

            migrationBuilder.DropIndex(
                name: "IX_Game_categoryId",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "categoryId",
                table: "Game");
        }
    }
}
