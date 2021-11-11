using Microsoft.EntityFrameworkCore.Migrations;

namespace JYTGameStore.Migrations
{
    public partial class EditAddressTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_Country_CountryCode",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_Address_Province_ProvinceCode",
                table: "Address");

            migrationBuilder.DropIndex(
                name: "IX_Address_CountryCode",
                table: "Address");

            migrationBuilder.DropIndex(
                name: "IX_Address_ProvinceCode",
                table: "Address");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Address_CountryCode",
                table: "Address",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Address_ProvinceCode",
                table: "Address",
                column: "ProvinceCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Country_CountryCode",
                table: "Address",
                column: "CountryCode",
                principalTable: "Country",
                principalColumn: "CountryCode",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Province_ProvinceCode",
                table: "Address",
                column: "ProvinceCode",
                principalTable: "Province",
                principalColumn: "ProvinceCode",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
