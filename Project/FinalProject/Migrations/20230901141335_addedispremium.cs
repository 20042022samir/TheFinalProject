using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Migrations
{
    /// <inheritdoc />
    public partial class addedispremium : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Houses_AspNetUsers_AppUserId",
                table: "Houses");

            migrationBuilder.DropIndex(
                name: "IX_Houses_AppUserId",
                table: "Houses");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "Houses",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "Houses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPremium",
                table: "Houses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Houses_AppUserId1",
                table: "Houses",
                column: "AppUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Houses_AspNetUsers_AppUserId1",
                table: "Houses",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Houses_AspNetUsers_AppUserId1",
                table: "Houses");

            migrationBuilder.DropIndex(
                name: "IX_Houses_AppUserId1",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "IsPremium",
                table: "Houses");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "Houses",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Houses_AppUserId",
                table: "Houses",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Houses_AspNetUsers_AppUserId",
                table: "Houses",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
