using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StoreStatusAsString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "restaurants",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.Sql(
                """
                    UPDATE restaurants as r1 SET status = (
                        CASE
                            WHEN status = '1' THEN 'Active'
                            WHEN status = '2' THEN 'Inactive'
                            WHEN status = '3' THEN 'Closed'
                            ELSE NULL
                        END
                    )
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                    UPDATE restaurants as r1 SET status = (
                        CASE
                            WHEN status = 'Active' THEN '1'
                            WHEN status = 'Inactive' THEN '2'
                            WHEN status = 'Closed' THEN '3'
                            ELSE NULL
                        END
                    )
                """);

            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "restaurants",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
