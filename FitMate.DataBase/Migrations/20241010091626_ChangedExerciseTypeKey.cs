using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitMate.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class ChangedExerciseTypeKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_ExerciseTypes_ExerciseTypeID",
                table: "Exercises");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExerciseTypes",
                table: "ExerciseTypes");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_ExerciseTypeID",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "ExerciseTypes");

            migrationBuilder.DropColumn(
                name: "ExerciseTypeID",
                table: "Exercises");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ExerciseTypes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ExerciseTypeName",
                table: "Exercises",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExerciseTypes",
                table: "ExerciseTypes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_ExerciseTypeName",
                table: "Exercises",
                column: "ExerciseTypeName");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_ExerciseTypes_ExerciseTypeName",
                table: "Exercises",
                column: "ExerciseTypeName",
                principalTable: "ExerciseTypes",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_ExerciseTypes_ExerciseTypeName",
                table: "Exercises");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExerciseTypes",
                table: "ExerciseTypes");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_ExerciseTypeName",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "ExerciseTypeName",
                table: "Exercises");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ExerciseTypes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "ExerciseTypes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ExerciseTypeID",
                table: "Exercises",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExerciseTypes",
                table: "ExerciseTypes",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_ExerciseTypeID",
                table: "Exercises",
                column: "ExerciseTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_ExerciseTypes_ExerciseTypeID",
                table: "Exercises",
                column: "ExerciseTypeID",
                principalTable: "ExerciseTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
