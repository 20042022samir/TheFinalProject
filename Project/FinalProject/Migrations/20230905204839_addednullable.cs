using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Migrations
{
    /// <inheritdoc />
    public partial class addednullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Houses_Cities_CityId",
                table: "Houses");

            migrationBuilder.DropForeignKey(
                name: "FK_Houses_Countries_CountryId",
                table: "Houses");

            migrationBuilder.DropForeignKey(
                name: "FK_Houses_Distincts_DistinctId",
                table: "Houses");

            migrationBuilder.DropForeignKey(
                name: "FK_Houses_Typees_TypeeId",
                table: "Houses");

            migrationBuilder.AlterColumn<int>(
                name: "TypeeId",
                table: "Houses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DistinctId",
                table: "Houses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "Houses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "Houses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Houses_Cities_CityId",
                table: "Houses",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Houses_Countries_CountryId",
                table: "Houses",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Houses_Distincts_DistinctId",
                table: "Houses",
                column: "DistinctId",
                principalTable: "Distincts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Houses_Typees_TypeeId",
                table: "Houses",
                column: "TypeeId",
                principalTable: "Typees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Houses_Cities_CityId",
                table: "Houses");

            migrationBuilder.DropForeignKey(
                name: "FK_Houses_Countries_CountryId",
                table: "Houses");

            migrationBuilder.DropForeignKey(
                name: "FK_Houses_Distincts_DistinctId",
                table: "Houses");

            migrationBuilder.DropForeignKey(
                name: "FK_Houses_Typees_TypeeId",
                table: "Houses");

            migrationBuilder.AlterColumn<int>(
                name: "TypeeId",
                table: "Houses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DistinctId",
                table: "Houses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "Houses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "Houses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Houses_Cities_CityId",
                table: "Houses",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Houses_Countries_CountryId",
                table: "Houses",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Houses_Distincts_DistinctId",
                table: "Houses",
                column: "DistinctId",
                principalTable: "Distincts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Houses_Typees_TypeeId",
                table: "Houses",
                column: "TypeeId",
                principalTable: "Typees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
