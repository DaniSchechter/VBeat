using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VBeat.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(maxLength: 20, nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(maxLength: 16, nullable: false),
                    DateOfRegistration = table.Column<DateTime>(nullable: false),
                    TimeOfLastLogin = table.Column<DateTime>(nullable: false),
                    ArtistName = table.Column<string>(maxLength: 20, nullable: false),
                    ArtistImage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Shows",
                columns: table => new
                {
                    ShowId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ShowName = table.Column<string>(maxLength: 20, nullable: false),
                    Country = table.Column<string>(maxLength: 15, nullable: false),
                    StreetName = table.Column<string>(maxLength: 20, nullable: false),
                    HouseNumber = table.Column<int>(nullable: false),
                    ShowTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shows", x => x.ShowId);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistModel",
                columns: table => new
                {
                    PlaylistId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Public = table.Column<bool>(nullable: false),
                    PlaylistImage = table.Column<string>(nullable: true),
                    ArtistModelUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistModel", x => x.PlaylistId);
                    table.ForeignKey(
                        name: "FK_PlaylistModel_Artists_ArtistModelUserId",
                        column: x => x.ArtistModelUserId,
                        principalTable: "Artists",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArtistShowModel",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    ShowId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistShowModel", x => new { x.ShowId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ArtistShowModel_Shows_ShowId",
                        column: x => x.ShowId,
                        principalTable: "Shows",
                        principalColumn: "ShowId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtistShowModel_Artists_UserId",
                        column: x => x.UserId,
                        principalTable: "Artists",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Songs",
                columns: table => new
                {
                    SongId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SongName = table.Column<string>(nullable: false),
                    Genre = table.Column<string>(nullable: false),
                    SongPath = table.Column<string>(nullable: false),
                    SongImagePath = table.Column<string>(nullable: true),
                    ReleaseDate = table.Column<DateTime>(nullable: false),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    PlaylistModelPlaylistId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Songs", x => x.SongId);
                    table.ForeignKey(
                        name: "FK_Songs_PlaylistModel_PlaylistModelPlaylistId",
                        column: x => x.PlaylistModelPlaylistId,
                        principalTable: "PlaylistModel",
                        principalColumn: "PlaylistId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArtistSongModel",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    SongId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistSongModel", x => new { x.SongId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ArtistSongModel_Songs_SongId",
                        column: x => x.SongId,
                        principalTable: "Songs",
                        principalColumn: "SongId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtistSongModel_Artists_UserId",
                        column: x => x.UserId,
                        principalTable: "Artists",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtistShowModel_UserId",
                table: "ArtistShowModel",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistSongModel_UserId",
                table: "ArtistSongModel",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistModel_ArtistModelUserId",
                table: "PlaylistModel",
                column: "ArtistModelUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_PlaylistModelPlaylistId",
                table: "Songs",
                column: "PlaylistModelPlaylistId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtistShowModel");

            migrationBuilder.DropTable(
                name: "ArtistSongModel");

            migrationBuilder.DropTable(
                name: "Shows");

            migrationBuilder.DropTable(
                name: "Songs");

            migrationBuilder.DropTable(
                name: "PlaylistModel");

            migrationBuilder.DropTable(
                name: "Artists");
        }
    }
}
