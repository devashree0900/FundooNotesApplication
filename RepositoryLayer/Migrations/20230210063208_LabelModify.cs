using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryLayer.Migrations
{
    public partial class LabelModify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabelTable_NoteTable_NoteID",
                table: "LabelTable");

            migrationBuilder.RenameColumn(
                name: "NoteID",
                table: "LabelTable",
                newName: "NoteId");

            migrationBuilder.RenameIndex(
                name: "IX_LabelTable_NoteID",
                table: "LabelTable",
                newName: "IX_LabelTable_NoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_LabelTable_NoteTable_NoteId",
                table: "LabelTable",
                column: "NoteId",
                principalTable: "NoteTable",
                principalColumn: "NoteId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabelTable_NoteTable_NoteId",
                table: "LabelTable");

            migrationBuilder.RenameColumn(
                name: "NoteId",
                table: "LabelTable",
                newName: "NoteID");

            migrationBuilder.RenameIndex(
                name: "IX_LabelTable_NoteId",
                table: "LabelTable",
                newName: "IX_LabelTable_NoteID");

            migrationBuilder.AddForeignKey(
                name: "FK_LabelTable_NoteTable_NoteID",
                table: "LabelTable",
                column: "NoteID",
                principalTable: "NoteTable",
                principalColumn: "NoteId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
