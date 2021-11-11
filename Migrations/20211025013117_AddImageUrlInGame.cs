using Microsoft.EntityFrameworkCore.Migrations;

namespace JYTGameStore.Migrations
{
    public partial class AddImageUrlInGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "imageUrl",
                table: "Game",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cart_gameId",
                table: "Cart",
                column: "gameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Game_gameId",
                table: "Cart",
                column: "gameId",
                principalTable: "Game",
                principalColumn: "gameId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Game_gameId",
                table: "Cart");

            migrationBuilder.DropIndex(
                name: "IX_Cart_gameId",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "imageUrl",
                table: "Game");
        }
    }
}
