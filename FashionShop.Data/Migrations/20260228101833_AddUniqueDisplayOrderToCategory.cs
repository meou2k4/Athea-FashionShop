using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FashionShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueDisplayOrderToCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Categories_DisplayOrder",
                table: "Categories",
                column: "DisplayOrder",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_DisplayOrder",
                table: "Categories");
        }
    }
}
