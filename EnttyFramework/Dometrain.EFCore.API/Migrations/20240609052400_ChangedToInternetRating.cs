using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dometrain.EFCore.API.Migrations
{
    /// <inheritdoc />
    public partial class ChangedToInternetRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ImdbRating",
                table: "Movies",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m
                );

            migrationBuilder.RenameColumn(
                name: "ImdbRating",
                table: "Movies",
                newName: "InternetRating");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "InternetRating",
                table: "Movies",
                type: "int",
                nullable: false,
                defaultValue: 0
                );

            migrationBuilder.RenameColumn(
                name: "InternetRating",
                table: "Movies",
                newName: "ImdbRating");
        }
    }
}
