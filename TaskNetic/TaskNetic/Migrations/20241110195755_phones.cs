using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskNetic.Migrations
{
    /// <inheritdoc />
    public partial class phones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
           name: "PhoneNumber",
           table: "ApplicationUsers",
           type: "text",
           nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "ApplicationUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "ApplicationUsers");
        }
    }
}
