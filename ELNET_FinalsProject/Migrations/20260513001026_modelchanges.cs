using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMDBSYS.Migrations
{
    /// <inheritdoc />
    public partial class modelchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Variation",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasVariation",
                table: "Menus",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Variation",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "HasVariation",
                table: "Menus");
        }
    }
}
