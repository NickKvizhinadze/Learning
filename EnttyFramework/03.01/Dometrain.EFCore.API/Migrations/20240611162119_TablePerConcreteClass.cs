using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dometrain.EFCore.API.Migrations
{
    /// <inheritdoc />
    public partial class TablePerConcreteClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CinemaMovies_Movies_Identifier",
                table: "CinemaMovies");

            migrationBuilder.DropForeignKey(
                name: "FK_TelevisionMovies_Movies_Identifier",
                table: "TelevisionMovies");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TelevisionMovies",
                table: "TelevisionMovies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CinemaMovies",
                table: "CinemaMovies");

            migrationBuilder.RenameTable(
                name: "TelevisionMovies",
                newName: "TelevisionMovie");

            migrationBuilder.RenameTable(
                name: "CinemaMovies",
                newName: "CinemaMovie");

            migrationBuilder.CreateSequence(
                name: "MovieSequence");

            migrationBuilder.AlterColumn<int>(
                name: "Identifier",
                table: "TelevisionMovie",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR [MovieSequence]",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "AgeRating",
                table: "TelevisionMovie",
                type: "char(32)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "InternetRating",
                table: "TelevisionMovie",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "MainGenreId",
                table: "TelevisionMovie",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Plot",
                table: "TelevisionMovie",
                type: "varchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReleaseDate",
                table: "TelevisionMovie",
                type: "char(8)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "TelevisionMovie",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Identifier",
                table: "CinemaMovie",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR [MovieSequence]",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "AgeRating",
                table: "CinemaMovie",
                type: "char(32)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "InternetRating",
                table: "CinemaMovie",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "MainGenreId",
                table: "CinemaMovie",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Plot",
                table: "CinemaMovie",
                type: "varchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReleaseDate",
                table: "CinemaMovie",
                type: "char(8)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "CinemaMovie",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TelevisionMovie",
                table: "TelevisionMovie",
                column: "Identifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CinemaMovie",
                table: "CinemaMovie",
                column: "Identifier");

            migrationBuilder.CreateIndex(
                name: "IX_TelevisionMovie_MainGenreId",
                table: "TelevisionMovie",
                column: "MainGenreId");

            migrationBuilder.CreateIndex(
                name: "IX_CinemaMovie_MainGenreId",
                table: "CinemaMovie",
                column: "MainGenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_CinemaMovie_Genres_MainGenreId",
                table: "CinemaMovie",
                column: "MainGenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TelevisionMovie_Genres_MainGenreId",
                table: "TelevisionMovie",
                column: "MainGenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CinemaMovie_Genres_MainGenreId",
                table: "CinemaMovie");

            migrationBuilder.DropForeignKey(
                name: "FK_TelevisionMovie_Genres_MainGenreId",
                table: "TelevisionMovie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TelevisionMovie",
                table: "TelevisionMovie");

            migrationBuilder.DropIndex(
                name: "IX_TelevisionMovie_MainGenreId",
                table: "TelevisionMovie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CinemaMovie",
                table: "CinemaMovie");

            migrationBuilder.DropIndex(
                name: "IX_CinemaMovie_MainGenreId",
                table: "CinemaMovie");

            migrationBuilder.DropColumn(
                name: "AgeRating",
                table: "TelevisionMovie");

            migrationBuilder.DropColumn(
                name: "InternetRating",
                table: "TelevisionMovie");

            migrationBuilder.DropColumn(
                name: "MainGenreId",
                table: "TelevisionMovie");

            migrationBuilder.DropColumn(
                name: "Plot",
                table: "TelevisionMovie");

            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "TelevisionMovie");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "TelevisionMovie");

            migrationBuilder.DropColumn(
                name: "AgeRating",
                table: "CinemaMovie");

            migrationBuilder.DropColumn(
                name: "InternetRating",
                table: "CinemaMovie");

            migrationBuilder.DropColumn(
                name: "MainGenreId",
                table: "CinemaMovie");

            migrationBuilder.DropColumn(
                name: "Plot",
                table: "CinemaMovie");

            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "CinemaMovie");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "CinemaMovie");

            migrationBuilder.DropSequence(
                name: "MovieSequence");

            migrationBuilder.RenameTable(
                name: "TelevisionMovie",
                newName: "TelevisionMovies");

            migrationBuilder.RenameTable(
                name: "CinemaMovie",
                newName: "CinemaMovies");

            migrationBuilder.AlterColumn<int>(
                name: "Identifier",
                table: "TelevisionMovies",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "NEXT VALUE FOR [MovieSequence]");

            migrationBuilder.AlterColumn<int>(
                name: "Identifier",
                table: "CinemaMovies",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "NEXT VALUE FOR [MovieSequence]");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TelevisionMovies",
                table: "TelevisionMovies",
                column: "Identifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CinemaMovies",
                table: "CinemaMovies",
                column: "Identifier");

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Identifier = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainGenreId = table.Column<int>(type: "int", nullable: false),
                    AgeRating = table.Column<string>(type: "char(32)", nullable: false),
                    InternetRating = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReleaseDate = table.Column<string>(type: "char(8)", nullable: false),
                    Plot = table.Column<string>(type: "varchar(max)", nullable: true),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_Movies_MainGenreId",
                table: "Movies",
                column: "MainGenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_CinemaMovies_Movies_Identifier",
                table: "CinemaMovies",
                column: "Identifier",
                principalTable: "Movies",
                principalColumn: "Identifier",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TelevisionMovies_Movies_Identifier",
                table: "TelevisionMovies",
                column: "Identifier",
                principalTable: "Movies",
                principalColumn: "Identifier",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
