using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lararium.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Video : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "media_streams",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_size = table.Column<long>(type: "bigint", nullable: false),
                    file_type = table.Column<string>(type: "text", nullable: false),
                    file_ext = table.Column<string>(type: "text", nullable: false),
                    buffer_path = table.Column<string>(type: "text", nullable: true),
                    in_buffer = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_media_streams", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "media_streams");
        }
    }
}
