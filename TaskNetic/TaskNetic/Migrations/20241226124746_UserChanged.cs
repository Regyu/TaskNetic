using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskNetic.Migrations
{
    /// <inheritdoc />
    public partial class UserChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserCard");

            migrationBuilder.AddColumn<int>(
                name: "CardId",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CardId",
                table: "AspNetUsers",
                column: "CardId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Cards_CardId",
                table: "AspNetUsers",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "CardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Cards_CardId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CardId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CardId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "ApplicationUserCard",
                columns: table => new
                {
                    CardMembersId = table.Column<string>(type: "text", nullable: false),
                    CardsCardId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserCard", x => new { x.CardMembersId, x.CardsCardId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserCard_AspNetUsers_CardMembersId",
                        column: x => x.CardMembersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserCard_Cards_CardsCardId",
                        column: x => x.CardsCardId,
                        principalTable: "Cards",
                        principalColumn: "CardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserCard_CardsCardId",
                table: "ApplicationUserCard",
                column: "CardsCardId");
        }
    }
}
