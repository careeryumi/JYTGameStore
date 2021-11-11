using Microsoft.EntityFrameworkCore.Migrations;

namespace JYTGameStore.Migrations
{
    public partial class DeleteCategoryIdInGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_Category_categoryId",
                table: "Game");

            migrationBuilder.RenameColumn(
                name: "categoryId",
                table: "Game",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Game_categoryId",
                table: "Game",
                newName: "IX_Game_CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Category_CategoryId",
                table: "Game",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_Category_CategoryId",
                table: "Game");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Game",
                newName: "categoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Game_CategoryId",
                table: "Game",
                newName: "IX_Game_categoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Category_categoryId",
                table: "Game",
                column: "categoryId",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
