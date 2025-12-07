using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fall2025_Final_Humbuckers.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSpotifyIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SpotifyId",
                table: "GlobalTracks",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SpotifyId",
                table: "FavoriteTracks",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpotifyId",
                table: "GlobalTracks");

            migrationBuilder.DropColumn(
                name: "SpotifyId",
                table: "FavoriteTracks");
        }
    }
}
