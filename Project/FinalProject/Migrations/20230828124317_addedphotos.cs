using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Migrations
{
    /// <inheritdoc />
    public partial class addedphotos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgentedHousePhoto_AgentedHouses_AgentedHouseId",
                table: "AgentedHousePhoto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AgentedHousePhoto",
                table: "AgentedHousePhoto");

            migrationBuilder.RenameTable(
                name: "AgentedHousePhoto",
                newName: "agentedHousePhotos");

            migrationBuilder.RenameIndex(
                name: "IX_AgentedHousePhoto_AgentedHouseId",
                table: "agentedHousePhotos",
                newName: "IX_agentedHousePhotos_AgentedHouseId");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "agentedHousePhotos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "agentedHousePhotos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_agentedHousePhotos",
                table: "agentedHousePhotos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_agentedHousePhotos_AgentedHouses_AgentedHouseId",
                table: "agentedHousePhotos",
                column: "AgentedHouseId",
                principalTable: "AgentedHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_agentedHousePhotos_AgentedHouses_AgentedHouseId",
                table: "agentedHousePhotos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_agentedHousePhotos",
                table: "agentedHousePhotos");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "agentedHousePhotos");

            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "agentedHousePhotos");

            migrationBuilder.RenameTable(
                name: "agentedHousePhotos",
                newName: "AgentedHousePhoto");

            migrationBuilder.RenameIndex(
                name: "IX_agentedHousePhotos_AgentedHouseId",
                table: "AgentedHousePhoto",
                newName: "IX_AgentedHousePhoto_AgentedHouseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AgentedHousePhoto",
                table: "AgentedHousePhoto",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AgentedHousePhoto_AgentedHouses_AgentedHouseId",
                table: "AgentedHousePhoto",
                column: "AgentedHouseId",
                principalTable: "AgentedHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
