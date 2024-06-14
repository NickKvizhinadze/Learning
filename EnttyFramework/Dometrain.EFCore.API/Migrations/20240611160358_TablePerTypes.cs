using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dometrain.EFCore.API.Migrations
{
    /// <inheritdoc />
    public partial class TablePerTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "CinemaMovies",
                columns: table => new
                {
                    Identifier = table.Column<int>(type: "int", nullable: false),
                    GrossRevenue = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CinemaMovies", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_CinemaMovies_Movies_Identifier",
                        column: x => x.Identifier,
                        principalTable: "Movies",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TelevisionMovies",
                columns: table => new
                {
                    Identifier = table.Column<int>(type: "int", nullable: false),
                    ChannelFirstAiredOn = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelevisionMovies", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_TelevisionMovies_Movies_Identifier",
                        column: x => x.Identifier,
                        principalTable: "Movies",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CinemaMovies");

            migrationBuilder.DropTable(
                name: "TelevisionMovies");

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
    }
}
