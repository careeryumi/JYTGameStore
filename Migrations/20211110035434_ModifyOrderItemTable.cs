using Microsoft.EntityFrameworkCore.Migrations;

namespace JYTGameStore.Migrations
{
    public partial class ModifyOrderItemTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CreditCard_CreditCardId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "CreditCardId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CreditCard_CreditCardId",
                table: "Orders",
                column: "CreditCardId",
                principalTable: "CreditCard",
                principalColumn: "CreditCardId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CreditCard_CreditCardId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "CreditCardId",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CreditCard_CreditCardId",
                table: "Orders",
                column: "CreditCardId",
                principalTable: "CreditCard",
                principalColumn: "CreditCardId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
