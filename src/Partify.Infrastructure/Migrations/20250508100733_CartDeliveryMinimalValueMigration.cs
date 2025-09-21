using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerServiceOnlineShop.Migrations
{
    /// <inheritdoc />
    public partial class CartDeliveryMinimalValueMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MinimalDeliveryValue",
                table: "Carts",
                type: "decimal(10,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinimalDeliveryValue",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Products",
                newName: "CartiD");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "ProductImages",
                newName: "CartiD");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "ProductCategories",
                newName: "CartiD");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Offers",
                newName: "CartiD");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "OfferDeliveryTypes",
                newName: "CartiD");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "DeliveryTypes",
                newName: "CartiD");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Countries",
                newName: "CartiD");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Conditions",
                newName: "CartiD");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Carts",
                newName: "CartiD");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "CartItems",
                newName: "CartiD");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "AspNetUsers",
                newName: "CartiD");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "AspNetUserClaims",
                newName: "CartiD");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "AspNetRoles",
                newName: "CartiD");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "AspNetRoleClaims",
                newName: "CartiD");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Addresses",
                newName: "CartiD");
        }
    }
}
