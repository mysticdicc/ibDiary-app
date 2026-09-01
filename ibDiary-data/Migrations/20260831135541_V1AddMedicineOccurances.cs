using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ibDiary_data.Migrations
{
    /// <inheritdoc />
    public partial class V1AddMedicineOccurances : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueAt",
                table: "MedicineReports");

            migrationBuilder.AddColumn<int>(
                name: "DueAtId",
                table: "MedicineReports",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MedicineDueAtOccurance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MedicineId = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    DueAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineDueAtOccurance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicineDueAtOccurance_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicineReports_DueAtId",
                table: "MedicineReports",
                column: "DueAtId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineDueAtOccurance_MedicineId",
                table: "MedicineDueAtOccurance",
                column: "MedicineId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineReports_MedicineDueAtOccurance_DueAtId",
                table: "MedicineReports",
                column: "DueAtId",
                principalTable: "MedicineDueAtOccurance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicineReports_MedicineDueAtOccurance_DueAtId",
                table: "MedicineReports");

            migrationBuilder.DropTable(
                name: "MedicineDueAtOccurance");

            migrationBuilder.DropIndex(
                name: "IX_MedicineReports_DueAtId",
                table: "MedicineReports");

            migrationBuilder.DropColumn(
                name: "DueAtId",
                table: "MedicineReports");

            migrationBuilder.AddColumn<DateTime>(
                name: "DueAt",
                table: "MedicineReports",
                type: "TEXT",
                nullable: true);
        }
    }
}
