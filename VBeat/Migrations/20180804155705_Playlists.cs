using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VBeat.Migrations
{
    public partial class Playlists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistModel_Artists_ArtistModelUserId",
                table: "PlaylistModel");

            migrationBuilder.DropForeignKey(
                name: "FK_Songs_PlaylistModel_PlaylistModelPlaylistId",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_Songs_PlaylistModelPlaylistId",
                table: "Songs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlaylistModel",
                table: "PlaylistModel");

            migrationBuilder.DropColumn(
                name: "PlaylistModelPlaylistId",
                table: "Songs");

            migrationBuilder.RenameTable(
                name: "PlaylistModel",
                newName: "Playlists");

            migrationBuilder.RenameIndex(
                name: "IX_PlaylistModel_ArtistModelUserId",
                table: "Playlists",
                newName: "IX_Playlists_ArtistModelUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Playlists",
                table: "Playlists",
                column: "PlaylistId");

            migrationBuilder.CreateTable(
                name: "PlaylistSongModel",
                columns: table => new
                {
                    PlaylistId = table.Column<int>(nullable: false),
                    SongId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistSongModel", x => new { x.PlaylistId, x.SongId });
                    table.ForeignKey(
                        name: "FK_PlaylistSongModel_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "PlaylistId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaylistSongModel_Songs_SongId",
                        column: x => x.SongId,
                        principalTable: "Songs",
                        principalColumn: "SongId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistSongModel_SongId",
                table: "PlaylistSongModel",
                column: "SongId");

            migrationBuilder.AddForeignKey(
                name: "FK_Playlists_Artists_ArtistModelUserId",
                table: "Playlists",
                column: "ArtistModelUserId",
                principalTable: "Artists",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Playlists_Artists_ArtistModelUserId",
                table: "Playlists");

            migrationBuilder.DropTable(
                name: "PlaylistSongModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Playlists",
                table: "Playlists");

            migrationBuilder.RenameTable(
                name: "Playlists",
                newName: "PlaylistModel");

            migrationBuilder.RenameIndex(
                name: "IX_Playlists_ArtistModelUserId",
                table: "PlaylistModel",
                newName: "IX_PlaylistModel_ArtistModelUserId");

            migrationBuilder.AddColumn<int>(
                name: "PlaylistModelPlaylistId",
                table: "Songs",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlaylistModel",
                table: "PlaylistModel",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_PlaylistModelPlaylistId",
                table: "Songs",
                column: "PlaylistModelPlaylistId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistModel_Artists_ArtistModelUserId",
                table: "PlaylistModel",
                column: "ArtistModelUserId",
                principalTable: "Artists",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_PlaylistModel_PlaylistModelPlaylistId",
                table: "Songs",
                column: "PlaylistModelPlaylistId",
                principalTable: "PlaylistModel",
                principalColumn: "PlaylistId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
