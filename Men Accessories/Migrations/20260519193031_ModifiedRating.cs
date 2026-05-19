using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Men_Accessories.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Rate");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Rate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Rate");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Rate",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
