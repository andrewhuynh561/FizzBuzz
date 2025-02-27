using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FizzBuzz.Migrations
{
    /// <inheritdoc />
    public partial class AddNumberRangeToGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxNumber",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinNumber",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxNumber",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "MinNumber",
                table: "Games");
        }
    }
}
