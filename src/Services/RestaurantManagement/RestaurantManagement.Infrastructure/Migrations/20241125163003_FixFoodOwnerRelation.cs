using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixFoodOwnerRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "owner_id",
                table: "foods",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_foods_owner_id",
                table: "foods",
                column: "owner_id");

            migrationBuilder.AddForeignKey(
                name: "fk_foods_restaurant_owners_owner_id",
                table: "foods",
                column: "owner_id",
                principalTable: "restaurant_owners",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_foods_restaurant_owners_owner_id",
                table: "foods");

            migrationBuilder.DropIndex(
                name: "ix_foods_owner_id",
                table: "foods");

            migrationBuilder.DropColumn(
                name: "owner_id",
                table: "foods");
        }
    }
}
