using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Men_Accessories.Migrations
{
    /// <inheritdoc />
    public partial class AddedFavoritesToCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FavoriteProductIds",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FavoriteProductIds",
                table: "Customers");
        }
    }
}
