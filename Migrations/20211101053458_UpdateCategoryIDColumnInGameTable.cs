using Microsoft.EntityFrameworkCore.Migrations;

namespace JYTGameStore.Migrations
{
    public partial class UpdateCategoryIDColumnInGameTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_Category_CategoryId",
                table: "Game");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Game",
                newName: "CategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_Game_CategoryId",
                table: "Game",
                newName: "IX_Game_CategoryID");

            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "Game",
                type: "real",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Category_CategoryID",
                table: "Game",
                column: "CategoryID",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_Category_CategoryID",
                table: "Game");

            migrationBuilder.RenameColumn(
                name: "CategoryID",
                table: "Game",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Game_CategoryID",
                table: "Game",
                newName: "IX_Game_CategoryId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Game",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Category_CategoryId",
                table: "Game",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
