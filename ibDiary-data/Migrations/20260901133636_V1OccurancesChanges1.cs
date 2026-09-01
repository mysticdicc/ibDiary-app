using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ibDiary_data.Migrations
{
    /// <inheritdoc />
    public partial class V1OccurancesChanges1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicineDueAtOccurance_Medicines_MedicineId",
                table: "MedicineDueAtOccurance");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicineReports_MedicineDueAtOccurance_DueAtId",
                table: "MedicineReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicineDueAtOccurance",
                table: "MedicineDueAtOccurance");

            migrationBuilder.RenameTable(
                name: "MedicineDueAtOccurance",
                newName: "MedicineOccurances");

            migrationBuilder.RenameIndex(
                name: "IX_MedicineDueAtOccurance_MedicineId",
                table: "MedicineOccurances",
                newName: "IX_MedicineOccurances_MedicineId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicineOccurances",
                table: "MedicineOccurances",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineOccurances_Medicines_MedicineId",
                table: "MedicineOccurances",
                column: "MedicineId",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineReports_MedicineOccurances_DueAtId",
                table: "MedicineReports",
                column: "DueAtId",
                principalTable: "MedicineOccurances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicineOccurances_Medicines_MedicineId",
                table: "MedicineOccurances");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicineReports_MedicineOccurances_DueAtId",
                table: "MedicineReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicineOccurances",
                table: "MedicineOccurances");

            migrationBuilder.RenameTable(
                name: "MedicineOccurances",
                newName: "MedicineDueAtOccurance");

            migrationBuilder.RenameIndex(
                name: "IX_MedicineOccurances_MedicineId",
                table: "MedicineDueAtOccurance",
                newName: "IX_MedicineDueAtOccurance_MedicineId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicineDueAtOccurance",
                table: "MedicineDueAtOccurance",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineDueAtOccurance_Medicines_MedicineId",
                table: "MedicineDueAtOccurance",
                column: "MedicineId",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineReports_MedicineDueAtOccurance_DueAtId",
                table: "MedicineReports",
                column: "DueAtId",
                principalTable: "MedicineDueAtOccurance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
