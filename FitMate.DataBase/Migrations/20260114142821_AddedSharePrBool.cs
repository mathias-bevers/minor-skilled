using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitMate.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class AddedSharePrBool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SharePR",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SharePR",
                table: "Users");
        }
    }
}
