using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskService.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class FixTypoInDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commnets_Tasks_TaskId",
                table: "Commnets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Commnets",
                table: "Commnets");

            migrationBuilder.RenameTable(
                name: "Commnets",
                newName: "Comments");

            migrationBuilder.RenameIndex(
                name: "IX_Commnets_TaskId",
                table: "Comments",
                newName: "IX_Comments_TaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Tasks_TaskId",
                table: "Comments",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Tasks_TaskId",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Commnets");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_TaskId",
                table: "Commnets",
                newName: "IX_Commnets_TaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Commnets",
                table: "Commnets",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Commnets_Tasks_TaskId",
                table: "Commnets",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
