using Microsoft.EntityFrameworkCore.Migrations;

namespace VBeat.Migrations
{
    public partial class UserMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtistShowModel_Artists_UserId",
                table: "ArtistShowModel");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistSongModel_Artists_UserId",
                table: "ArtistSongModel");

            migrationBuilder.DropForeignKey(
                name: "FK_Playlists_Artists_ArtistModelUserId",
                table: "Playlists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Artists",
                table: "Artists");

            migrationBuilder.RenameTable(
                name: "Artists",
                newName: "Users");

            migrationBuilder.RenameColumn(
                name: "ArtistModelUserId",
                table: "Playlists",
                newName: "UserModelUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Playlists_ArtistModelUserId",
                table: "Playlists",
                newName: "IX_Playlists_UserModelUserId");

            migrationBuilder.AlterColumn<string>(
                name: "ArtistName",
                table: "Users",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Users",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistShowModel_Users_UserId",
                table: "ArtistShowModel",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistSongModel_Users_UserId",
                table: "ArtistSongModel",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_ArtistShowModel_Users_UserId",
                table: "ArtistShowModel");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistSongModel_Users_UserId",
                table: "ArtistSongModel");

            migrationBuilder.DropForeignKey(
                name: "FK_Playlists_Users_UserModelUserId",
                table: "Playlists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Artists");

            migrationBuilder.RenameColumn(
                name: "UserModelUserId",
                table: "Playlists",
                newName: "ArtistModelUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Playlists_UserModelUserId",
                table: "Playlists",
                newName: "IX_Playlists_ArtistModelUserId");

            migrationBuilder.AlterColumn<string>(
                name: "ArtistName",
                table: "Artists",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Artists",
                table: "Artists",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistShowModel_Artists_UserId",
                table: "ArtistShowModel",
                column: "UserId",
                principalTable: "Artists",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistSongModel_Artists_UserId",
                table: "ArtistSongModel",
                column: "UserId",
                principalTable: "Artists",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Playlists_Artists_ArtistModelUserId",
                table: "Playlists",
                column: "ArtistModelUserId",
                principalTable: "Artists",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
