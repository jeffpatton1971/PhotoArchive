using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhotoArchive.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPhotoImportFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Gallery",
                table: "Photos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostId",
                table: "Photos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostUrl",
                table: "Photos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SortIndex",
                table: "Photos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceFilename",
                table: "Photos",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gallery",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "PostUrl",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "SortIndex",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "SourceFilename",
                table: "Photos");
        }
    }
}
