using System.Collections.ObjectModel;
using System.Data;
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
        string serverConnectionString = App.SETTINGS.Server.ConnectionString;   
        await using SqlConnection connection = new(serverConnectionString);
        await using SqlCommand command = new(GenerateWorkoutQuery(), connection);

        try
        {
            if (connection.State != ConnectionState.Open) { connection.Open(); }

            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Workout workout = new()
                    {
                        CreatedOn = Convert.ToString(reader["CreatedOn"]) ??
                                    throw new SqlNullValueException("reader[\"CreatedOn\"]"),
                        ID = Convert.ToInt32(reader["ID"]),
                        MusclesWorked = Convert.ToString(reader["MusclesWorked"]) ??
                                        throw new SqlNullValueException("reader[\"MusclesWorked\"]")
                    };


                    workout.MusclesWorked = "Muscles Worked: " + workout.MusclesWorked;
                    Workouts.Add(workout);
                }
            }
        }
        catch (Exception e) { System.Diagnostics.Debug.WriteLine(e.Message); }

        connection.Close();
    }

    private string GenerateWorkoutQuery() =>
        "SELECT w.CreatedOn, w.ID, COALESCE(STRING_AGG(MG.Name,\t', '), 'No Exercises logged') AS MusclesWorked " +
        "FROM Workouts w LEFT JOIN ( Select DISTINCT mg.Name, e.WorkoutID " +
        "FROM Exercises e JOIN ExerciseTypes et ON e.ExerciseTypeName = et.Name " +
        "JOIN MuscleGroups mg ON et.MuscleGroupID = mg.ID) MG ON MG.WorkoutID = w.ID " +
        $"WHERE w.UserID = {App.USER_ID} GROUP BY w.CreatedOn, w.ID;";
}