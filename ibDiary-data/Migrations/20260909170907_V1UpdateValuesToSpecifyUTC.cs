using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ibDiary_data.Migrations
{
    /// <inheritdoc />
    public partial class V1UpdateValuesToSpecifyUTC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartedAt",
                table: "Symptoms",
                newName: "StartedAtUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Symptoms",
                newName: "CreatedAtUtc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartedAtUtc",
                table: "Symptoms",
                newName: "StartedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAtUtc",
                table: "Symptoms",
                newName: "CreatedAt");
        }
    }
}
