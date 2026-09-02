using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ibDiary_data.Migrations
{
    /// <inheritdoc />
    public partial class V1AddingStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StatsSnapshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MonthEnd = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    MedicineCount = table.Column<int>(type: "INTEGER", nullable: false),
                    ActiveMedicineCount = table.Column<int>(type: "INTEGER", nullable: false),
                    SymptomCount = table.Column<int>(type: "INTEGER", nullable: false),
                    ActiveSymptomCount = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalMedicineReports = table.Column<int>(type: "INTEGER", nullable: false),
                    MonthlyMedicalReports = table.Column<int>(type: "INTEGER", nullable: false),
                    MonthlyMedicinesTaken = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalSymptomReports = table.Column<int>(type: "INTEGER", nullable: false),
                    MonthlySymptomReports = table.Column<int>(type: "INTEGER", nullable: false),
                    UniqueMonthlyFoodItems = table.Column<int>(type: "INTEGER", nullable: false),
                    UniqueMonthlyMeals = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalFoodReports = table.Column<int>(type: "INTEGER", nullable: false),
                    MonthlyFoodReports = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalMealReports = table.Column<int>(type: "INTEGER", nullable: false),
                    MonthlyMealReports = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatsSnapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FoodStatsSnapshot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FoodId = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalReportsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    MonthlyReportsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    StatsSnapshotId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodStatsSnapshot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoodStatsSnapshot_FoodItems_FoodId",
                        column: x => x.FoodId,
                        principalTable: "FoodItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoodStatsSnapshot_StatsSnapshots_StatsSnapshotId",
                        column: x => x.StatsSnapshotId,
                        principalTable: "StatsSnapshots",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MealStatsSnapshot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MealId = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalReportsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    MonthlyReportsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    StatsSnapshotId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealStatsSnapshot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealStatsSnapshot_Meals_MealId",
                        column: x => x.MealId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealStatsSnapshot_StatsSnapshots_StatsSnapshotId",
                        column: x => x.StatsSnapshotId,
                        principalTable: "StatsSnapshots",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MedicineStatsSnapshot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MedicineId = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalReportsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    MonthlyReportsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalStateChanges = table.Column<int>(type: "INTEGER", nullable: false),
                    MonthlyStateChanges = table.Column<int>(type: "INTEGER", nullable: false),
                    StatsSnapshotId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineStatsSnapshot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicineStatsSnapshot_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicineStatsSnapshot_StatsSnapshots_StatsSnapshotId",
                        column: x => x.StatsSnapshotId,
                        principalTable: "StatsSnapshots",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SymptomStatsSnapshot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SymptomId = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalReportsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    MonthlyReportsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalStateChanges = table.Column<int>(type: "INTEGER", nullable: false),
                    MonthlyStateChanges = table.Column<int>(type: "INTEGER", nullable: false),
                    StatsSnapshotId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymptomStatsSnapshot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SymptomStatsSnapshot_StatsSnapshots_StatsSnapshotId",
                        column: x => x.StatsSnapshotId,
                        principalTable: "StatsSnapshots",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SymptomStatsSnapshot_Symptoms_SymptomId",
                        column: x => x.SymptomId,
                        principalTable: "Symptoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FoodEatenTrendPoint",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    StartHour = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    Count = table.Column<int>(type: "INTEGER", nullable: false),
                    FoodStatsSnapshotId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodEatenTrendPoint", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoodEatenTrendPoint_FoodStatsSnapshot_FoodStatsSnapshotId",
                        column: x => x.FoodStatsSnapshotId,
                        principalTable: "FoodStatsSnapshot",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MealEatenTrendPoint",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    StartHour = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    Count = table.Column<int>(type: "INTEGER", nullable: false),
                    MealStatsSnapshotId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealEatenTrendPoint", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealEatenTrendPoint_MealStatsSnapshot_MealStatsSnapshotId",
                        column: x => x.MealStatsSnapshotId,
                        principalTable: "MealStatsSnapshot",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MedicineTakenTrendPoint",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    AverageTaken = table.Column<double>(type: "REAL", nullable: false),
                    ReportCount = table.Column<int>(type: "INTEGER", nullable: false),
                    MedicineStatsSnapshotId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineTakenTrendPoint", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicineTakenTrendPoint_MedicineStatsSnapshot_MedicineStatsSnapshotId",
                        column: x => x.MedicineStatsSnapshotId,
                        principalTable: "MedicineStatsSnapshot",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SymptomSeverityTrendPoint",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    AverageSeverity = table.Column<double>(type: "REAL", nullable: false),
                    ReportCount = table.Column<int>(type: "INTEGER", nullable: false),
                    SymptomStatsSnapshotId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymptomSeverityTrendPoint", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SymptomSeverityTrendPoint_SymptomStatsSnapshot_SymptomStatsSnapshotId",
                        column: x => x.SymptomStatsSnapshotId,
                        principalTable: "SymptomStatsSnapshot",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodEatenTrendPoint_FoodStatsSnapshotId",
                table: "FoodEatenTrendPoint",
                column: "FoodStatsSnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodStatsSnapshot_FoodId",
                table: "FoodStatsSnapshot",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodStatsSnapshot_StatsSnapshotId",
                table: "FoodStatsSnapshot",
                column: "StatsSnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_MealEatenTrendPoint_MealStatsSnapshotId",
                table: "MealEatenTrendPoint",
                column: "MealStatsSnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_MealStatsSnapshot_MealId",
                table: "MealStatsSnapshot",
                column: "MealId");

            migrationBuilder.CreateIndex(
                name: "IX_MealStatsSnapshot_StatsSnapshotId",
                table: "MealStatsSnapshot",
                column: "StatsSnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineStatsSnapshot_MedicineId",
                table: "MedicineStatsSnapshot",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineStatsSnapshot_StatsSnapshotId",
                table: "MedicineStatsSnapshot",
                column: "StatsSnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineTakenTrendPoint_MedicineStatsSnapshotId",
                table: "MedicineTakenTrendPoint",
                column: "MedicineStatsSnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_StatsSnapshots_MonthEnd",
                table: "StatsSnapshots",
                column: "MonthEnd",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SymptomSeverityTrendPoint_SymptomStatsSnapshotId",
                table: "SymptomSeverityTrendPoint",
                column: "SymptomStatsSnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomStatsSnapshot_StatsSnapshotId",
                table: "SymptomStatsSnapshot",
                column: "StatsSnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomStatsSnapshot_SymptomId",
                table: "SymptomStatsSnapshot",
                column: "SymptomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodEatenTrendPoint");

            migrationBuilder.DropTable(
                name: "MealEatenTrendPoint");

            migrationBuilder.DropTable(
                name: "MedicineTakenTrendPoint");

            migrationBuilder.DropTable(
                name: "SymptomSeverityTrendPoint");

            migrationBuilder.DropTable(
                name: "FoodStatsSnapshot");

            migrationBuilder.DropTable(
                name: "MealStatsSnapshot");

            migrationBuilder.DropTable(
                name: "MedicineStatsSnapshot");

            migrationBuilder.DropTable(
                name: "SymptomStatsSnapshot");

            migrationBuilder.DropTable(
                name: "StatsSnapshots");
        }
    }
}
