﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace VBeat.Migrations
{
    public partial class PlaylistLazyLoading : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Playlists_Users_UserModelUserId",
                table: "Playlists");

            migrationBuilder.AlterColumn<int>(
                name: "UserModelUserId",
                table: "Playlists",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Playlists_Users_UserModelUserId",
                table: "Playlists",
                column: "UserModelUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Playlists_Users_UserModelUserId",
                table: "Playlists");

            migrationBuilder.AlterColumn<int>(
                name: "UserModelUserId",
                table: "Playlists",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Playlists_Users_UserModelUserId",
                table: "Playlists",
                column: "UserModelUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
