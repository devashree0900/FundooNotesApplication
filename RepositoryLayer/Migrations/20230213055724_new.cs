using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryLayer.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTrahed",
                table: "NoteTable");

            migrationBuilder.AddColumn<bool>(
                name: "IsTrashed",
                table: "NoteTable",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTrashed",
                table: "NoteTable");

            migrationBuilder.AddColumn<bool>(
                name: "IsTrahed",
                table: "NoteTable",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
