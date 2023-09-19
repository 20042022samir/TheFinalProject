using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Migrations
{
    /// <inheritdoc />
    public partial class addedbooleans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ForAnswer",
                table: "Messages",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ForDelete",
                table: "Messages",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForAnswer",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ForDelete",
                table: "Messages");
        }
    }
}
