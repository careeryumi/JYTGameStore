using Microsoft.EntityFrameworkCore.Migrations;

namespace JYTGameStore.Migrations
{
    public partial class AddCreditCardToOrdersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreditCardId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CreditCardId",
                table: "Orders",
                column: "CreditCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CreditCard_CreditCardId",
                table: "Orders",
                column: "CreditCardId",
                principalTable: "CreditCard",
                principalColumn: "CreditCardId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CreditCard_CreditCardId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CreditCardId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CreditCardId",
                table: "Orders");
        }
    }
}
