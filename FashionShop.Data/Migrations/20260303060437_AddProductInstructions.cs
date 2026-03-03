using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FashionShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductInstructions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StorageInstructions",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "StorageInstructions",
                table: "Products");
        }
    }
}
