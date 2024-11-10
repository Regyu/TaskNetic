using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskNetic.Migrations
{
    /// <inheritdoc />
    public partial class addField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
            name: "ImagePath",
            table: "ApplicationUsers",
            type: "text",
            nullable: true);

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectRoles_AspNetRoles_ApplicationRoleId",
                table: "ProjectRoles");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationRoleId",
                table: "ProjectRoles",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "AspNetRoles",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetRoles",
                type: "character varying(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectRoles_AspNetRoles_ApplicationRoleId",
                table: "ProjectRoles",
                column: "ApplicationRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "ImagePath",
            table: "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectRoles_AspNetRoles_ApplicationRoleId",
                table: "ProjectRoles");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetRoles");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationRoleId",
                table: "ProjectRoles",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "AspNetRoles",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectRoles_AspNetRoles_ApplicationRoleId",
                table: "ProjectRoles",
                column: "ApplicationRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
        }
    }
}
