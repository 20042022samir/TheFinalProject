using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Migrations
{
    /// <inheritdoc />
    public partial class updatedmails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "roomCount",
                table: "IntrestMessages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TypeId",
                table: "IntrestMessages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "IntrestMessages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "IntrestMessages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "IntrestMessages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_IntrestMessages_CityId",
                table: "IntrestMessages",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_IntrestMessages_CountryId",
                table: "IntrestMessages",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_IntrestMessages_TypeId",
                table: "IntrestMessages",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_IntrestMessages_Cities_CityId",
                table: "IntrestMessages",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IntrestMessages_Countries_CountryId",
                table: "IntrestMessages",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IntrestMessages_Typees_TypeId",
                table: "IntrestMessages",
                column: "TypeId",
                principalTable: "Typees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IntrestMessages_Cities_CityId",
                table: "IntrestMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_IntrestMessages_Countries_CountryId",
                table: "IntrestMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_IntrestMessages_Typees_TypeId",
                table: "IntrestMessages");

            migrationBuilder.DropIndex(
                name: "IX_IntrestMessages_CityId",
                table: "IntrestMessages");

            migrationBuilder.DropIndex(
                name: "IX_IntrestMessages_CountryId",
                table: "IntrestMessages");

            migrationBuilder.DropIndex(
                name: "IX_IntrestMessages_TypeId",
                table: "IntrestMessages");

            migrationBuilder.AlterColumn<int>(
                name: "roomCount",
                table: "IntrestMessages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TypeId",
                table: "IntrestMessages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "IntrestMessages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "IntrestMessages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "IntrestMessages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
