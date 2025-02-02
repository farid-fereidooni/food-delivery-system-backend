using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "event_logs",
                columns: table => new
                {
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_name = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: false),
                    event_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    transaction_id = table.Column<Guid>(type: "uuid", nullable: false),
                    times_sent = table.Column<int>(type: "integer", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    state = table.Column<string>(type: "text", nullable: false),
                    topic = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_event_logs", x => x.event_id);
                });

            migrationBuilder.CreateTable(
                name: "files",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_name = table.Column<string>(type: "text", nullable: false),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    mime_type = table.Column<string>(type: "text", nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    group = table.Column<string>(type: "text", nullable: true),
                    is_temporary = table.Column<bool>(type: "boolean", nullable: false),
                    owner_permission = table.Column<byte>(type: "smallint", nullable: false),
                    group_permission = table.Column<byte>(type: "smallint", nullable: false),
                    other_permission = table.Column<byte>(type: "smallint", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_files", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_event_logs_state_transaction_id",
                table: "event_logs",
                columns: new[] { "state", "transaction_id" });

            migrationBuilder.CreateIndex(
                name: "ix_files_group_group_permission",
                table: "files",
                columns: new[] { "group", "group_permission" });

            migrationBuilder.CreateIndex(
                name: "ix_files_is_temporary",
                table: "files",
                column: "is_temporary");

            migrationBuilder.CreateIndex(
                name: "ix_files_owner_id_owner_permission",
                table: "files",
                columns: new[] { "owner_id", "owner_permission" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "event_logs");

            migrationBuilder.DropTable(
                name: "files");
        }
    }
}
