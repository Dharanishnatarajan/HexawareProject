using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotPot.Migrations
{
    /// <inheritdoc />
    public partial class Validations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuCategories_Restaurants_RestaurantId",
                table: "MenuCategories");

            migrationBuilder.DropIndex(
                name: "IX_MenuCategories_RestaurantId",
                table: "MenuCategories");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "MenuCategories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RestaurantId",
                table: "MenuCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MenuCategories_RestaurantId",
                table: "MenuCategories",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuCategories_Restaurants_RestaurantId",
                table: "MenuCategories",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
