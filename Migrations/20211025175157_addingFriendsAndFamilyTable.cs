using Microsoft.EntityFrameworkCore.Migrations;

namespace JYTGameStore.Migrations
{
    public partial class addingFriendsAndFamilyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FriendsAndFamily",
                columns: table => new
                {
                    friendsAndFamilyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    memberEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FriendsAndFamilyEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isFriendOrFamily = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bothAccepted = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendsAndFamily", x => x.friendsAndFamilyId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendsAndFamily");
        }
    }
}
