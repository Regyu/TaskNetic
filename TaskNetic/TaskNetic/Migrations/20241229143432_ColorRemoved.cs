using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TaskNetic.Migrations
{
    /// <inheritdoc />
    public partial class ColorRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Labels_Colors_ColorId",
                table: "Labels");

            migrationBuilder.DropForeignKey(
                name: "FK_TodoTasks_Cards_CardId",
                table: "TodoTasks");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropIndex(
                name: "IX_Labels_ColorId",
                table: "Labels");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "Labels");

            migrationBuilder.RenameColumn(
                name: "comment",
                table: "Labels",
                newName: "Comment");

            migrationBuilder.AlterColumn<int>(
                name: "CardId",
                table: "TodoTasks",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "ColorCode",
                table: "Labels",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoTasks_Cards_CardId",
                table: "TodoTasks",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "CardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoTasks_Cards_CardId",
                table: "TodoTasks");

            migrationBuilder.DropColumn(
                name: "ColorCode",
                table: "Labels");

            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "Labels",
                newName: "comment");

            migrationBuilder.AlterColumn<int>(
                name: "CardId",
                table: "TodoTasks",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "Labels",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ColorName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Labels_ColorId",
                table: "Labels",
                column: "ColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Labels_Colors_ColorId",
                table: "Labels",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoTasks_Cards_CardId",
                table: "TodoTasks",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "CardId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
