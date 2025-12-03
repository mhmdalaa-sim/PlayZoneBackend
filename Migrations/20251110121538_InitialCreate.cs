using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlayZone.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "admin_users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admin_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rooms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    available = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rooms", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "whatsapp_config",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    business_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_whatsapp_config", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "blocked_slots",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    room_id = table.Column<int>(type: "integer", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    start_time = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    end_time = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blocked_slots", x => x.id);
                    table.ForeignKey(
                        name: "FK_blocked_slots_rooms_room_id",
                        column: x => x.room_id,
                        principalTable: "rooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bookings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    room_id = table.Column<int>(type: "integer", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    start_time = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    end_time = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    duration = table.Column<decimal>(type: "numeric", nullable: false),
                    customer_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    customer_phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookings", x => x.id);
                    table.ForeignKey(
                        name: "FK_bookings_rooms_room_id",
                        column: x => x.room_id,
                        principalTable: "rooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "admin_users",
                columns: new[] { "id", "created_at", "password_hash", "username" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 11, 10, 12, 15, 36, 435, DateTimeKind.Utc).AddTicks(6199), "$2a$11$YPInZtySVXY/u6LPHiyMIOj.XnL0z.NMvgUhKul7EpgGgnjCq6ch6", "admin" });

            migrationBuilder.InsertData(
                table: "rooms",
                columns: new[] { "id", "available", "created_at", "description", "name", "updated_at" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2025, 11, 10, 12, 15, 36, 312, DateTimeKind.Utc).AddTicks(8828), "PlayStation Gaming Room 1", "Room 1", new DateTime(2025, 11, 10, 12, 15, 36, 312, DateTimeKind.Utc).AddTicks(8829) },
                    { 2, true, new DateTime(2025, 11, 10, 12, 15, 36, 312, DateTimeKind.Utc).AddTicks(8832), "PlayStation Gaming Room 2", "Room 2", new DateTime(2025, 11, 10, 12, 15, 36, 312, DateTimeKind.Utc).AddTicks(8832) },
                    { 3, true, new DateTime(2025, 11, 10, 12, 15, 36, 312, DateTimeKind.Utc).AddTicks(8834), "PlayStation Gaming Room 3", "Room 3", new DateTime(2025, 11, 10, 12, 15, 36, 312, DateTimeKind.Utc).AddTicks(8835) },
                    { 4, true, new DateTime(2025, 11, 10, 12, 15, 36, 312, DateTimeKind.Utc).AddTicks(8837), "PlayStation Gaming Room 4", "Room 4", new DateTime(2025, 11, 10, 12, 15, 36, 312, DateTimeKind.Utc).AddTicks(8837) },
                    { 5, true, new DateTime(2025, 11, 10, 12, 15, 36, 312, DateTimeKind.Utc).AddTicks(8839), "PlayStation Gaming Room 5", "Room 5", new DateTime(2025, 11, 10, 12, 15, 36, 312, DateTimeKind.Utc).AddTicks(8839) },
                    { 6, true, new DateTime(2025, 11, 10, 12, 15, 36, 312, DateTimeKind.Utc).AddTicks(8842), "PlayStation Gaming Room 6", "Room 6", new DateTime(2025, 11, 10, 12, 15, 36, 312, DateTimeKind.Utc).AddTicks(8842) },
                    { 7, true, new DateTime(2025, 11, 10, 12, 15, 36, 312, DateTimeKind.Utc).AddTicks(8845), "PlayStation Gaming Room 7", "Room 7", new DateTime(2025, 11, 10, 12, 15, 36, 312, DateTimeKind.Utc).AddTicks(8845) },
                    { 8, true, new DateTime(2025, 11, 10, 12, 15, 36, 312, DateTimeKind.Utc).AddTicks(8847), "PlayStation Gaming Room 8", "Room 8", new DateTime(2025, 11, 10, 12, 15, 36, 312, DateTimeKind.Utc).AddTicks(8847) }
                });

            migrationBuilder.InsertData(
                table: "whatsapp_config",
                columns: new[] { "id", "business_number", "updated_at" },
                values: new object[] { 1, "+201234567890", new DateTime(2025, 11, 10, 12, 15, 36, 435, DateTimeKind.Utc).AddTicks(7837) });

            migrationBuilder.CreateIndex(
                name: "IX_admin_users_username",
                table: "admin_users",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_blocked_slots_room_id_date",
                table: "blocked_slots",
                columns: new[] { "room_id", "date" });

            migrationBuilder.CreateIndex(
                name: "IX_blocked_slots_room_id_date_start_time_end_time",
                table: "blocked_slots",
                columns: new[] { "room_id", "date", "start_time", "end_time" });

            migrationBuilder.CreateIndex(
                name: "IX_bookings_date_start_time_end_time",
                table: "bookings",
                columns: new[] { "date", "start_time", "end_time" });

            migrationBuilder.CreateIndex(
                name: "IX_bookings_room_id_date",
                table: "bookings",
                columns: new[] { "room_id", "date" });

            migrationBuilder.CreateIndex(
                name: "IX_bookings_room_id_date_start_time_end_time",
                table: "bookings",
                columns: new[] { "room_id", "date", "start_time", "end_time" });

            migrationBuilder.CreateIndex(
                name: "IX_rooms_name",
                table: "rooms",
                column: "name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admin_users");

            migrationBuilder.DropTable(
                name: "blocked_slots");

            migrationBuilder.DropTable(
                name: "bookings");

            migrationBuilder.DropTable(
                name: "whatsapp_config");

            migrationBuilder.DropTable(
                name: "rooms");
        }
    }
}
