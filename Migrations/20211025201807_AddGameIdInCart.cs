using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JYTGameStore.Migrations
{
    public partial class AddGameIdInCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "gameId",
                table: "Cart",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "regDate",
                table: "Cart",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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
                onDelete: ReferentialAction.Restrict);
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
                name: "gameId",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "regDate",
                table: "Cart");
        }
    }
}
