using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IntegrateFoodInMenuItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "food_type_food");

            migrationBuilder.DropTable(
                name: "foods");

            migrationBuilder.DropIndex(
                name: "ix_menu_items_menu_id_category_id_food_id",
                table: "menu_items");

            migrationBuilder.DropColumn(
                name: "food_id",
                table: "menu_items");

            migrationBuilder.AddColumn<string>(
                name: "specification_description",
                table: "menu_items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "specification_name",
                table: "menu_items",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "specification_price",
                table: "menu_items",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "food_type_menu_item",
                columns: table => new
                {
                    food_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    menu_item_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_food_type_menu_item", x => new { x.food_type_id, x.menu_item_id });
                    table.ForeignKey(
                        name: "fk_food_type_menu_item_food_types_food_type_id",
                        column: x => x.food_type_id,
                        principalTable: "food_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_food_type_menu_item_menu_items_menu_item_id",
                        column: x => x.menu_item_id,
                        principalTable: "menu_items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_menu_items_menu_id",
                table: "menu_items",
                column: "menu_id");

            migrationBuilder.CreateIndex(
                name: "ix_food_type_menu_item_menu_item_id",
                table: "food_type_menu_item",
                column: "menu_item_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "food_type_menu_item");

            migrationBuilder.DropIndex(
                name: "ix_menu_items_menu_id",
                table: "menu_items");

            migrationBuilder.DropColumn(
                name: "specification_description",
                table: "menu_items");

            migrationBuilder.DropColumn(
                name: "specification_name",
                table: "menu_items");

            migrationBuilder.DropColumn(
                name: "specification_price",
                table: "menu_items");

            migrationBuilder.AddColumn<Guid>(
                name: "food_id",
                table: "menu_items",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "foods",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    specification_description = table.Column<string>(type: "text", nullable: true),
                    specification_name = table.Column<string>(type: "text", nullable: false),
                    specification_price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_foods", x => x.id);
                    table.ForeignKey(
                        name: "fk_foods_restaurant_owners_owner_id",
                        column: x => x.owner_id,
                        principalTable: "restaurant_owners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "food_type_food",
                columns: table => new
                {
                    food_id = table.Column<Guid>(type: "uuid", nullable: false),
                    food_type_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_food_type_food", x => new { x.food_id, x.food_type_id });
                    table.ForeignKey(
                        name: "fk_food_type_food_food_types_food_type_id",
                        column: x => x.food_type_id,
                        principalTable: "food_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_food_type_food_foods_food_id",
                        column: x => x.food_id,
                        principalTable: "foods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_menu_items_menu_id_category_id_food_id",
                table: "menu_items",
                columns: new[] { "menu_id", "category_id", "food_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_food_type_food_food_type_id",
                table: "food_type_food",
                column: "food_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_foods_owner_id",
                table: "foods",
                column: "owner_id");
        }
    }
}
