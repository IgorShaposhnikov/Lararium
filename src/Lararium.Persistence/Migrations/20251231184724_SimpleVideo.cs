using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lararium.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SimpleVideo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "media_streams");

            migrationBuilder.CreateTable(
                name: "videos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    summary = table.Column<string>(type: "text", nullable: false),
                    file_size = table.Column<long>(type: "bigint", nullable: false),
                    file_type = table.Column<string>(type: "text", nullable: false),
                    file_ext = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_videos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "media_actors",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    birthday = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    video_entity_id = table.Column<Guid>(type: "uuid", nullable: true),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_actors", x => x.id);
                    table.ForeignKey(
                        name: "fk_actors_videos_video_entity_id",
                        column: x => x.video_entity_id,
                        principalTable: "videos",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "media_tags",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    video_entity_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_media_tags", x => x.id);
                    table.ForeignKey(
                        name: "fk_media_tags_videos_video_entity_id",
                        column: x => x.video_entity_id,
                        principalTable: "videos",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_actors_video_entity_id",
                table: "media_actors",
                column: "video_entity_id");

            migrationBuilder.CreateIndex(
                name: "ix_media_tags_video_entity_id",
                table: "media_tags",
                column: "video_entity_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "media_actors");

            migrationBuilder.DropTable(
                name: "media_tags");

            migrationBuilder.DropTable(
                name: "videos");

            migrationBuilder.CreateTable(
                name: "media_streams",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    buffer_path = table.Column<string>(type: "text", nullable: true),
                    file_ext = table.Column<string>(type: "text", nullable: false),
                    file_size = table.Column<long>(type: "bigint", nullable: false),
                    file_type = table.Column<string>(type: "text", nullable: false),
                    in_buffer = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_media_streams", x => x.id);
                });
        }
    }
}
