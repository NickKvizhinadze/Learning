using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dometrain.EFCore.API.Migrations
{
    /// <inheritdoc />
    public partial class TablePerHierarchy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TelevisionMovie_Genres_MainGenreId",
                table: "TelevisionMovie");

            migrationBuilder.DropTable(
                name: "CinemaMovie");

            migrationBuilder.DropSequence(
                name: "MovieSequence");

            migrationBuilder.RenameTable(
                name: "TelevisionMovie",
                newName: "Movies");

            migrationBuilder.RenameIndex(
                name: "IX_TelevisionMovie_MainGenreId",
                table: "Movies",
                newName: "IX_Movies_MainGenreId");

            migrationBuilder.AlterColumn<int>(
                name: "MainGenreId",
                table: "Movies",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ChannelFirstAiredOn",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "Movies");
            
            migrationBuilder.AddColumn<int>(
                name: "Identifier",
                table: "Movies",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_Movies",
                table: "Movies",
                column: "Identifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Genres_MainGenreId",
                table: "Movies",
                column: "MainGenreId",
                principalTable: "Genres",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Genres_MainGenreId",
                table: "Movies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Movies",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "GrossRevenue",
                table: "Movies");

            migrationBuilder.RenameTable(
                name: "Movies",
                newName: "TelevisionMovie");

            migrationBuilder.RenameIndex(
                name: "IX_Movies_MainGenreId",
                table: "TelevisionMovie",
                newName: "IX_TelevisionMovie_MainGenreId");

            migrationBuilder.CreateSequence(
                name: "MovieSequence");

            migrationBuilder.AlterColumn<int>(
                name: "MainGenreId",
                table: "TelevisionMovie",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ChannelFirstAiredOn",
                table: "TelevisionMovie",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Identifier",
                table: "TelevisionMovie",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR [MovieSequence]",
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TelevisionMovie",
                table: "TelevisionMovie",
                column: "Identifier");

            migrationBuilder.CreateTable(
                name: "CinemaMovie",
                columns: table => new
                {
                    Identifier = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [MovieSequence]"),
                    MainGenreId = table.Column<int>(type: "int", nullable: false),
                    AgeRating = table.Column<string>(type: "char(32)", nullable: false),
                    InternetRating = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReleaseDate = table.Column<string>(type: "char(8)", nullable: false),
                    Plot = table.Column<string>(type: "varchar(max)", nullable: true),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    GrossRevenue = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CinemaMovie", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_CinemaMovie_Genres_MainGenreId",
                        column: x => x.MainGenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CinemaMovie_MainGenreId",
                table: "CinemaMovie",
                column: "MainGenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_TelevisionMovie_Genres_MainGenreId",
                table: "TelevisionMovie",
                column: "MainGenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
