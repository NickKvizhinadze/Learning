using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dometrain.EFCore.API.Migrations
{
    /// <inheritdoc />
    public partial class InheritanceStarted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movie_Actors");

            migrationBuilder.DropTable(
                name: "Movie_Directors");

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Identifier",
                keyValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "ChannelFirstAiredOn",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Movies",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "GrossRevenue",
                table: "Movies",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChannelFirstAiredOn",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "GrossRevenue",
                table: "Movies");

            migrationBuilder.CreateTable(
                name: "Movie_Actors",
                columns: table => new
                {
                    MovieIdentifier = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie_Actors", x => new { x.MovieIdentifier, x.Id });
                    table.ForeignKey(
                        name: "FK_Movie_Actors_Movies_MovieIdentifier",
                        column: x => x.MovieIdentifier,
                        principalTable: "Movies",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Movie_Directors",
                columns: table => new
                {
                    MovieIdentifier = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie_Directors", x => x.MovieIdentifier);
                    table.ForeignKey(
                        name: "FK_Movie_Directors_Movies_MovieIdentifier",
                        column: x => x.MovieIdentifier,
                        principalTable: "Movies",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Identifier", "AgeRating", "InternetRating", "MainGenreId", "ReleaseDate", "Plot", "Title" },
                values: new object[] { 1, "Adolescent", 0m, 1, "19990910", "Ed Norton and Brad Pitt have a couple of fist fights with each other.", "Fight Club" });

            migrationBuilder.InsertData(
                table: "Movie_Actors",
                columns: new[] { "Id", "MovieIdentifier", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, 1, "Edward", "Norton" },
                    { 2, 1, "Brad", "Pitt" }
                });

            migrationBuilder.InsertData(
                table: "Movie_Directors",
                columns: new[] { "MovieIdentifier", "FirstName", "LastName" },
                values: new object[] { 1, "David", "Fincher" });
        }
    }
}
