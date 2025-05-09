﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEventLog : Migration
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

            migrationBuilder.CreateIndex(
                name: "ix_event_logs_state_transaction_id",
                table: "event_logs",
                columns: new[] { "state", "transaction_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "event_logs");
        }
    }
}
