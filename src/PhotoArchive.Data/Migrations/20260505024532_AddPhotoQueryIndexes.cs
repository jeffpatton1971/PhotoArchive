using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhotoArchive.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPhotoQueryIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Photos_Gallery",
                table: "Photos",
                column: "Gallery");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_PostId",
                table: "Photos",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_Source",
                table: "Photos",
                column: "Source");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_TakenAt",
                table: "Photos",
                column: "TakenAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Photos_Gallery",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_PostId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_Source",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_TakenAt",
                table: "Photos");
        }
    }
}
