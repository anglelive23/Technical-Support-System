using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway.API.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRefreshTokenToIndependantTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Users_ApplicationUserId",
                schema: "security",
                table: "RefreshToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshToken",
                schema: "security",
                table: "RefreshToken");

            migrationBuilder.RenameTable(
                name: "RefreshToken",
                schema: "security",
                newName: "RefreshToken");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "RefreshToken",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshToken",
                table: "RefreshToken",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_ApplicationUserId",
                table: "RefreshToken",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_Users_ApplicationUserId",
                table: "RefreshToken",
                column: "ApplicationUserId",
                principalSchema: "security",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Users_ApplicationUserId",
                table: "RefreshToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshToken",
                table: "RefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_RefreshToken_ApplicationUserId",
                table: "RefreshToken");

            migrationBuilder.RenameTable(
                name: "RefreshToken",
                newName: "RefreshToken",
                newSchema: "security");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                schema: "security",
                table: "RefreshToken",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshToken",
                schema: "security",
                table: "RefreshToken",
                columns: new[] { "ApplicationUserId", "Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_Users_ApplicationUserId",
                schema: "security",
                table: "RefreshToken",
                column: "ApplicationUserId",
                principalSchema: "security",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
