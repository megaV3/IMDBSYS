using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELNET_FinalsProject.Migrations
{
    /// <inheritdoc />
    public partial class AddImagePathToMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Menus");
        }
    }
}
