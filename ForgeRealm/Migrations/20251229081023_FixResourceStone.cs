using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForgeRealm.Migrations
{
    /// <inheritdoc />
    public partial class FixResourceStone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Stone",
                table: "Resources",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stone",
                table: "Resources");
        }
    }
}
