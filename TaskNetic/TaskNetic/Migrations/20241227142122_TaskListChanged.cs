using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TaskNetic.Migrations
{
    /// <inheritdoc />
    public partial class TaskListChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_TaskLists_TaskListId",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_TodoTasks_TaskLists_TaskListId",
                table: "TodoTasks");

            migrationBuilder.DropTable(
                name: "TaskLists");

            migrationBuilder.DropIndex(
                name: "IX_Cards_TaskListId",
                table: "Cards");

            //migrationBuilder.DropColumn(
            //    name: "timestamp",
            //    table: "Comments");

            migrationBuilder.DropColumn(
                name: "TaskListId",
                table: "Cards");

            migrationBuilder.RenameColumn(
                name: "TaskListId",
                table: "TodoTasks",
                newName: "CardId");

            migrationBuilder.RenameIndex(
                name: "IX_TodoTasks_TaskListId",
                table: "TodoTasks",
                newName: "IX_TodoTasks_CardId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoTasks_Cards_CardId",
                table: "TodoTasks",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "CardId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoTasks_Cards_CardId",
                table: "TodoTasks");

            migrationBuilder.RenameColumn(
                name: "CardId",
                table: "TodoTasks",
                newName: "TaskListId");

            migrationBuilder.RenameIndex(
                name: "IX_TodoTasks_CardId",
                table: "TodoTasks",
                newName: "IX_TodoTasks_TaskListId");

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "timestamp",
            //    table: "Comments",
            //    type: "timestamp with time zone",
            //    nullable: false,
            //    defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TaskListId",
                table: "Cards",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TaskLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskLists", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_TaskListId",
                table: "Cards",
                column: "TaskListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_TaskLists_TaskListId",
                table: "Cards",
                column: "TaskListId",
                principalTable: "TaskLists",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoTasks_TaskLists_TaskListId",
                table: "TodoTasks",
                column: "TaskListId",
                principalTable: "TaskLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
