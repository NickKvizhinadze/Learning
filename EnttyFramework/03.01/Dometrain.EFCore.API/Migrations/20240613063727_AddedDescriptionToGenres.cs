using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dometrain.EFCore.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedDescriptionToGenres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Genres",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Movies_AgeRating",
                table: "Movies",
                column: "AgeRating",
                descending: new bool[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Movies_AgeRating",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Genres");
        }
    }
}
