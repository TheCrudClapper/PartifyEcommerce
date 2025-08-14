using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerServiceOnlineShop.Migrations
{
    /// <inheritdoc />
    public partial class LikedOfferRelationshipMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LikedOffers_OfferId",
                table: "LikedOffers",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_LikedOffers_UserId",
                table: "LikedOffers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LikedOffers_AspNetUsers_UserId",
                table: "LikedOffers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LikedOffers_Offers_OfferId",
                table: "LikedOffers",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LikedOffers_AspNetUsers_UserId",
                table: "LikedOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_LikedOffers_Offers_OfferId",
                table: "LikedOffers");

            migrationBuilder.DropIndex(
                name: "IX_LikedOffers_OfferId",
                table: "LikedOffers");

            migrationBuilder.DropIndex(
                name: "IX_LikedOffers_UserId",
                table: "LikedOffers");
        }
    }
}
