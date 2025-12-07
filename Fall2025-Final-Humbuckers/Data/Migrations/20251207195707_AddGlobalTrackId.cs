using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fall2025_Final_Humbuckers.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGlobalTrackId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Genre",
                table: "GlobalTracks");

            migrationBuilder.AlterColumn<string>(
                name: "SpotifyId",
                table: "FavoriteTracks",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "GlobalTrackId",
                table: "FavoriteTracks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GlobalTrackId",
                table: "FavoriteTracks");

            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "GlobalTracks",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "SpotifyId",
                table: "FavoriteTracks",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);
        }
    }
}
