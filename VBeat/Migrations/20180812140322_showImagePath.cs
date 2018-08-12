using Microsoft.EntityFrameworkCore.Migrations;

namespace VBeat.Migrations
{
    public partial class showImagePath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShowImagePath",
                table: "Shows",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowImagePath",
                table: "Shows");
        }
    }
}
