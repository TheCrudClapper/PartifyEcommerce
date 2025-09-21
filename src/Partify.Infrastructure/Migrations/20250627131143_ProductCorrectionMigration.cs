using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerServiceOnlineShop.Migrations
{
    /// <inheritdoc />
    public partial class ProductCorrectionMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Products",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ProductImages",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ProductCategories",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Offers",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "OfferDeliveryTypes",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "DeliveryTypes",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Countries",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Conditions",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Carts",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CartItems",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AspNetUsers",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AspNetUserClaims",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AspNetRoles",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AspNetRoleClaims",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Addresses",
                newName: "Value");
        }
    }
}
