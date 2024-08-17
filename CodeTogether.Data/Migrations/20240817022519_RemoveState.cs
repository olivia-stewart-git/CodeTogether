using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeTogether.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GM_GameState",
                table: "Games");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GM_GameState",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
