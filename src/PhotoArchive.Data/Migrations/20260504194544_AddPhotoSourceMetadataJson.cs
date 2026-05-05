using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhotoArchive.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPhotoSourceMetadataJson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SourceMetadataJson",
                table: "Photos",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceMetadataJson",
                table: "Photos");
        }
    }
}
