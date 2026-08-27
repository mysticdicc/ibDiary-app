using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ibDiary_data.Migrations
{
    /// <inheritdoc />
    public partial class V1AddValidation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNew",
                table: "SymptomStateChanges",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsNew",
                table: "MedicineStateChanges",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNew",
                table: "SymptomStateChanges");

            migrationBuilder.DropColumn(
                name: "IsNew",
                table: "MedicineStateChanges");
        }
    }
}
