using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JYTGameStore.Migrations
{
    public partial class UpdateCreditCardTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CreditCard",
                columns: table => new
                {
                    CreditCardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CCNumber = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    CCMonth = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    CCYear = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    CCPIN = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCard", x => x.CreditCardId);
                    table.ForeignKey(
                        name: "FK_CreditCard_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            /*
            migrationBuilder.CreateTable(
                name: "RegisterEvents",
                columns: table => new
                {
                    RegisterEventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisterEvents", x => x.RegisterEventId);
                    table.ForeignKey(
                        name: "FK_RegisterEvents_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "eventId",
                        onDelete: ReferentialAction.Cascade);
                });
            */

            migrationBuilder.CreateIndex(
                name: "IX_CreditCard_userId",
                table: "CreditCard",
                column: "userId");

            /*
            migrationBuilder.CreateIndex(
                name: "IX_RegisterEvents_EventId",
                table: "RegisterEvents",
                column: "EventId");
            */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditCard");

            /* migrationBuilder.DropTable(
                name: "RegisterEvents"); */
        }
    }
}
