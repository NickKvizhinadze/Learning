using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dometrain.EFCore.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Identifier = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    ReleaseDate = table.Column<string>(type: "char(8)", nullable: false),
                    Plot = table.Column<string>(type: "varchar(max)", nullable: true),
                    AgeRating = table.Column<string>(type: "char(32)", nullable: false),
                    MainGenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_Movies_Genres_MainGenreId",
                        column: x => x.MainGenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                table: "Genres",
                columns: new[] { "Id", "CreatedAt", "Name" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Drama" });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Identifier", "AgeRating", "MainGenreId", "ReleaseDate", "Plot", "Title" },
                values: new object[] { 1, "Adolescent", 1, "19990910", "Ed Norton and Brad Pitt have a couple of fist fights with each other.", "Fight Club" });

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

            migrationBuilder.CreateIndex(
                name: "IX_Movies_MainGenreId",
                table: "Movies",
                column: "MainGenreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movie_Actors");

            migrationBuilder.DropTable(
                name: "Movie_Directors");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Genres");
        }
    }
}
