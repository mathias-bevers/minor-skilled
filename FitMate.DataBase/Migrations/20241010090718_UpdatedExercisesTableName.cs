using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitMate.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedExercisesTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercise_ExercisesTypes_ExerciseTypeID",
                table: "Exercise");

            migrationBuilder.DropForeignKey(
                name: "FK_Exercise_Workouts_WorkoutID",
                table: "Exercise");

            migrationBuilder.DropForeignKey(
                name: "FK_ExercisesTypes_MeasurementTypes_MeasurementTypeID",
                table: "ExercisesTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_ExercisesTypes_MuscleGroups_MuscleGroupID",
                table: "ExercisesTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExercisesTypes",
                table: "ExercisesTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exercise",
                table: "Exercise");

            migrationBuilder.RenameTable(
                name: "ExercisesTypes",
                newName: "ExerciseTypes");

            migrationBuilder.RenameTable(
                name: "Exercise",
                newName: "Exercises");

            migrationBuilder.RenameIndex(
                name: "IX_ExercisesTypes_MuscleGroupID",
                table: "ExerciseTypes",
                newName: "IX_ExerciseTypes_MuscleGroupID");

            migrationBuilder.RenameIndex(
                name: "IX_ExercisesTypes_MeasurementTypeID",
                table: "ExerciseTypes",
                newName: "IX_ExerciseTypes_MeasurementTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_Exercise_WorkoutID",
                table: "Exercises",
                newName: "IX_Exercises_WorkoutID");

            migrationBuilder.RenameIndex(
                name: "IX_Exercise_ExerciseTypeID",
                table: "Exercises",
                newName: "IX_Exercises_ExerciseTypeID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExerciseTypes",
                table: "ExerciseTypes",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exercises",
                table: "Exercises",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_ExerciseTypes_ExerciseTypeID",
                table: "Exercises",
                column: "ExerciseTypeID",
                principalTable: "ExerciseTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Workouts_WorkoutID",
                table: "Exercises",
                column: "WorkoutID",
                principalTable: "Workouts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseTypes_MeasurementTypes_MeasurementTypeID",
                table: "ExerciseTypes",
                column: "MeasurementTypeID",
                principalTable: "MeasurementTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseTypes_MuscleGroups_MuscleGroupID",
                table: "ExerciseTypes",
                column: "MuscleGroupID",
                principalTable: "MuscleGroups",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_ExerciseTypes_ExerciseTypeID",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Workouts_WorkoutID",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseTypes_MeasurementTypes_MeasurementTypeID",
                table: "ExerciseTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseTypes_MuscleGroups_MuscleGroupID",
                table: "ExerciseTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExerciseTypes",
                table: "ExerciseTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exercises",
                table: "Exercises");

            migrationBuilder.RenameTable(
                name: "ExerciseTypes",
                newName: "ExercisesTypes");

            migrationBuilder.RenameTable(
                name: "Exercises",
                newName: "Exercise");

            migrationBuilder.RenameIndex(
                name: "IX_ExerciseTypes_MuscleGroupID",
                table: "ExercisesTypes",
                newName: "IX_ExercisesTypes_MuscleGroupID");

            migrationBuilder.RenameIndex(
                name: "IX_ExerciseTypes_MeasurementTypeID",
                table: "ExercisesTypes",
                newName: "IX_ExercisesTypes_MeasurementTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_Exercises_WorkoutID",
                table: "Exercise",
                newName: "IX_Exercise_WorkoutID");

            migrationBuilder.RenameIndex(
                name: "IX_Exercises_ExerciseTypeID",
                table: "Exercise",
                newName: "IX_Exercise_ExerciseTypeID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExercisesTypes",
                table: "ExercisesTypes",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exercise",
                table: "Exercise",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercise_ExercisesTypes_ExerciseTypeID",
                table: "Exercise",
                column: "ExerciseTypeID",
                principalTable: "ExercisesTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exercise_Workouts_WorkoutID",
                table: "Exercise",
                column: "WorkoutID",
                principalTable: "Workouts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExercisesTypes_MeasurementTypes_MeasurementTypeID",
                table: "ExercisesTypes",
                column: "MeasurementTypeID",
                principalTable: "MeasurementTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExercisesTypes_MuscleGroups_MuscleGroupID",
                table: "ExercisesTypes",
                column: "MuscleGroupID",
                principalTable: "MuscleGroups",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
