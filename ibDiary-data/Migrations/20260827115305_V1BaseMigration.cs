using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ibDiary_data.Migrations
{
    /// <inheritdoc />
    public partial class V1BaseMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalendarDays",
                columns: table => new
                {
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    IsNew = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarDays", x => x.Date);
                });

            migrationBuilder.CreateTable(
                name: "MedicineSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    IntervalType = table.Column<int>(type: "INTEGER", nullable: false),
                    IntervalValue = table.Column<int>(type: "INTEGER", nullable: false),
                    AmountPerDay = table.Column<int>(type: "INTEGER", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsNew = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineSchedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Meals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsNew = table.Column<bool>(type: "INTEGER", nullable: false),
                    CalendarDayDate = table.Column<DateOnly>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meals_CalendarDays_CalendarDayDate",
                        column: x => x.CalendarDayDate,
                        principalTable: "CalendarDays",
                        principalColumn: "Date");
                });

            migrationBuilder.CreateTable(
                name: "ScheduledNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    StartAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastSentAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IntervalType = table.Column<int>(type: "INTEGER", nullable: false),
                    IntervalValue = table.Column<int>(type: "INTEGER", nullable: false),
                    IsNew = table.Column<bool>(type: "INTEGER", nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    CalendarDayDate = table.Column<DateOnly>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduledNotifications_CalendarDays_CalendarDayDate",
                        column: x => x.CalendarDayDate,
                        principalTable: "CalendarDays",
                        principalColumn: "Date");
                });

            migrationBuilder.CreateTable(
                name: "Symptoms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsNew = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CalendarDayDate = table.Column<DateOnly>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Symptoms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Symptoms_CalendarDays_CalendarDayDate",
                        column: x => x.CalendarDayDate,
                        principalTable: "CalendarDays",
                        principalColumn: "Date");
                });

            migrationBuilder.CreateTable(
                name: "Medicines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Dose = table.Column<string>(type: "TEXT", nullable: false),
                    PrescribedBy = table.Column<string>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false),
                    PrescribedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MedicineScheduleId = table.Column<int>(type: "INTEGER", nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsNew = table.Column<bool>(type: "INTEGER", nullable: false),
                    CalendarDayDate = table.Column<DateOnly>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medicines_CalendarDays_CalendarDayDate",
                        column: x => x.CalendarDayDate,
                        principalTable: "CalendarDays",
                        principalColumn: "Date");
                    table.ForeignKey(
                        name: "FK_Medicines_MedicineSchedules_MedicineScheduleId",
                        column: x => x.MedicineScheduleId,
                        principalTable: "MedicineSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FoodItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsNew = table.Column<bool>(type: "INTEGER", nullable: false),
                    CalendarDayDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    MealId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoodItems_CalendarDays_CalendarDayDate",
                        column: x => x.CalendarDayDate,
                        principalTable: "CalendarDays",
                        principalColumn: "Date");
                    table.ForeignKey(
                        name: "FK_FoodItems_Meals_MealId",
                        column: x => x.MealId,
                        principalTable: "Meals",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MealReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MealId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AteMealAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false),
                    IsNew = table.Column<bool>(type: "INTEGER", nullable: false),
                    CalendarDayDate = table.Column<DateOnly>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealReports_CalendarDays_CalendarDayDate",
                        column: x => x.CalendarDayDate,
                        principalTable: "CalendarDays",
                        principalColumn: "Date");
                    table.ForeignKey(
                        name: "FK_MealReports_Meals_MealId",
                        column: x => x.MealId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SymptomStateChanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SymptomId = table.Column<int>(type: "INTEGER", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SymptomBefore = table.Column<string>(type: "jsonb", nullable: false),
                    SymptomAfter = table.Column<string>(type: "jsonb", nullable: false),
                    CalendarDayDate = table.Column<DateOnly>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymptomStateChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SymptomStateChanges_CalendarDays_CalendarDayDate",
                        column: x => x.CalendarDayDate,
                        principalTable: "CalendarDays",
                        principalColumn: "Date");
                    table.ForeignKey(
                        name: "FK_SymptomStateChanges_Symptoms_SymptomId",
                        column: x => x.SymptomId,
                        principalTable: "Symptoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicineReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MedicineId = table.Column<int>(type: "INTEGER", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MedicineTakenAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DueAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MedicineTaken = table.Column<bool>(type: "INTEGER", nullable: false),
                    Dose = table.Column<string>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false),
                    IsNew = table.Column<bool>(type: "INTEGER", nullable: false),
                    CalendarDayDate = table.Column<DateOnly>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicineReports_CalendarDays_CalendarDayDate",
                        column: x => x.CalendarDayDate,
                        principalTable: "CalendarDays",
                        principalColumn: "Date");
                    table.ForeignKey(
                        name: "FK_MedicineReports_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicineStateChanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MedicineId = table.Column<int>(type: "INTEGER", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MedicineBefore = table.Column<string>(type: "jsonb", nullable: false),
                    MedicineAfter = table.Column<string>(type: "jsonb", nullable: false),
                    CalendarDayDate = table.Column<DateOnly>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineStateChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicineStateChanges_CalendarDays_CalendarDayDate",
                        column: x => x.CalendarDayDate,
                        principalTable: "CalendarDays",
                        principalColumn: "Date");
                    table.ForeignKey(
                        name: "FK_MedicineStateChanges_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SymptomReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SymptomId = table.Column<int>(type: "INTEGER", nullable: false),
                    MedicationId = table.Column<int>(type: "INTEGER", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SubmittedFor = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Severity = table.Column<int>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false),
                    IsNew = table.Column<bool>(type: "INTEGER", nullable: false),
                    CalendarDayDate = table.Column<DateOnly>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymptomReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SymptomReports_CalendarDays_CalendarDayDate",
                        column: x => x.CalendarDayDate,
                        principalTable: "CalendarDays",
                        principalColumn: "Date");
                    table.ForeignKey(
                        name: "FK_SymptomReports_Medicines_MedicationId",
                        column: x => x.MedicationId,
                        principalTable: "Medicines",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SymptomReports_Symptoms_SymptomId",
                        column: x => x.SymptomId,
                        principalTable: "Symptoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FoodReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FoodItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AteFoodAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false),
                    IsNew = table.Column<bool>(type: "INTEGER", nullable: false),
                    CalendarDayDate = table.Column<DateOnly>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoodReports_CalendarDays_CalendarDayDate",
                        column: x => x.CalendarDayDate,
                        principalTable: "CalendarDays",
                        principalColumn: "Date");
                    table.ForeignKey(
                        name: "FK_FoodReports_FoodItems_FoodItemId",
                        column: x => x.FoodItemId,
                        principalTable: "FoodItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodItems_CalendarDayDate",
                table: "FoodItems",
                column: "CalendarDayDate");

            migrationBuilder.CreateIndex(
                name: "IX_FoodItems_MealId",
                table: "FoodItems",
                column: "MealId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodReports_CalendarDayDate",
                table: "FoodReports",
                column: "CalendarDayDate");

            migrationBuilder.CreateIndex(
                name: "IX_FoodReports_FoodItemId",
                table: "FoodReports",
                column: "FoodItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MealReports_CalendarDayDate",
                table: "MealReports",
                column: "CalendarDayDate");

            migrationBuilder.CreateIndex(
                name: "IX_MealReports_MealId",
                table: "MealReports",
                column: "MealId");

            migrationBuilder.CreateIndex(
                name: "IX_Meals_CalendarDayDate",
                table: "Meals",
                column: "CalendarDayDate");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineReports_CalendarDayDate",
                table: "MedicineReports",
                column: "CalendarDayDate");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineReports_MedicineId",
                table: "MedicineReports",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_CalendarDayDate",
                table: "Medicines",
                column: "CalendarDayDate");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_MedicineScheduleId",
                table: "Medicines",
                column: "MedicineScheduleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicineStateChanges_CalendarDayDate",
                table: "MedicineStateChanges",
                column: "CalendarDayDate");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineStateChanges_MedicineId",
                table: "MedicineStateChanges",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledNotifications_CalendarDayDate",
                table: "ScheduledNotifications",
                column: "CalendarDayDate");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomReports_CalendarDayDate",
                table: "SymptomReports",
                column: "CalendarDayDate");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomReports_MedicationId",
                table: "SymptomReports",
                column: "MedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomReports_SymptomId",
                table: "SymptomReports",
                column: "SymptomId");

            migrationBuilder.CreateIndex(
                name: "IX_Symptoms_CalendarDayDate",
                table: "Symptoms",
                column: "CalendarDayDate");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomStateChanges_CalendarDayDate",
                table: "SymptomStateChanges",
                column: "CalendarDayDate");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomStateChanges_SymptomId",
                table: "SymptomStateChanges",
                column: "SymptomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodReports");

            migrationBuilder.DropTable(
                name: "MealReports");

            migrationBuilder.DropTable(
                name: "MedicineReports");

            migrationBuilder.DropTable(
                name: "MedicineStateChanges");

            migrationBuilder.DropTable(
                name: "ScheduledNotifications");

            migrationBuilder.DropTable(
                name: "SymptomReports");

            migrationBuilder.DropTable(
                name: "SymptomStateChanges");

            migrationBuilder.DropTable(
                name: "FoodItems");

            migrationBuilder.DropTable(
                name: "Medicines");

            migrationBuilder.DropTable(
                name: "Symptoms");

            migrationBuilder.DropTable(
                name: "Meals");

            migrationBuilder.DropTable(
                name: "MedicineSchedules");

            migrationBuilder.DropTable(
                name: "CalendarDays");
        }
    }
}
