using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VBeat.Migrations
{
    public partial class Playlist_Name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlaylistName",
                table: "Playlists",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlaylistName",
                table: "Playlists");
        }
    }
}
