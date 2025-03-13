using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tofft_preprod.Migrations
{
    /// <inheritdoc />
    public partial class MissionRelationshipsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Missions_BoardId",
                table: "Missions",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Missions_Boards_BoardId",
                table: "Missions",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Missions_Boards_BoardId",
                table: "Missions");

            migrationBuilder.DropIndex(
                name: "IX_Missions_BoardId",
                table: "Missions");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Missions",
                newName: "AdminId");
        }
    }
}
