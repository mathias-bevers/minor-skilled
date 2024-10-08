using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using CommunityToolkit.Mvvm.ComponentModel;
using FitMate.Models;
using Microsoft.Data.SqlClient;

namespace FitMate.ViewModels;

public class AllWorkoutsViewModel : ObservableObject
{
    public ObservableCollection<Workout> Workouts { get; set; } = [];

    public void OnAppearing()
    {
        Task.Run(LoadFromDbAsync);
    }

    private async Task LoadFromDbAsync()
    {
        Workouts.Clear();
        await using SqlConnection connection = new(App.SERVER_SETTINGS.ConnectionString);

        connection.Open();

        await using (SqlCommand command = new(GenerateWorkoutQuery(), connection))
        {
            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Workout workout = new()
                    {
                        CreatedOn = Convert.ToString(reader["CreatedOn"]) ??
                                    throw new SqlNullValueException("reader[\"CreatedOn\"]"),
                        ID = Convert.ToInt32(reader["wID"]),
                        MusclesWorked = Convert.ToString(reader["MusclesWorked"]) ?? string.Empty
                    };

                    if (!string.IsNullOrEmpty(workout.MusclesWorked))
                    {
                        workout.MusclesWorked = "Muscles Worked: " + workout.MusclesWorked;
                    }

                    Workouts.Add(workout);
                }
            }
        }

        connection.Close();
    }

    private string GenerateWorkoutQuery() =>
        "SELECT w.CreatedOn, STRING_AGG(MG.Name, ', ') AS MusclesWorked, w.ID as wID FROM Workouts w " +
        "INNER JOIN (Select DISTINCT mg.Name, e.WorkoutID FROM Exercise e " +
        "JOIN ExercisesTypes et ON e.ExerciseTypeID = et.ID " +
        "JOIN MuscleGroups mg ON et.MuscleGroupID = mg.ID) MG " +
        $"ON MG.WorkoutID = w.ID WHERE w.UserID = {App.USER_ID} GROUP BY w.CreatedOn, w.ID;";
}