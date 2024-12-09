using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskNetic.Migrations
{
    /// <inheritdoc />
    public partial class NotificationChanged2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MentionedUserName",
                table: "Notifications",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MentionedUserName",
                table: "Notifications");
        }
    }
}
