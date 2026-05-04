using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhotoArchive.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    TakenAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Year = table.Column<int>(type: "integer", nullable: true),
                    Month = table.Column<int>(type: "integer", nullable: true),
                    Day = table.Column<int>(type: "integer", nullable: true),
                    Source = table.Column<string>(type: "text", nullable: false),
                    OriginalUrl = table.Column<string>(type: "text", nullable: false),
                    ThumbUrl = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Photos_Month_Day",
                table: "Photos",
                columns: new[] { "Month", "Day" });

            migrationBuilder.CreateIndex(
                name: "IX_Photos_Slug",
                table: "Photos",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photos_Year_Month_Day",
                table: "Photos",
                columns: new[] { "Year", "Month", "Day" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Photos");
        }
    }
}
