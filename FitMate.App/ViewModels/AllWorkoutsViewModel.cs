using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using CommunityToolkit.Mvvm.ComponentModel;
using FitMate.Models;
using FitMate.Utils;
using Microsoft.Data.SqlClient;
using Debug = System.Diagnostics.Debug;

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

        System.Text.StringBuilder sb = new("SELECT w.CreatedOn, w.ID, ");
        sb.Append("COALESCE(STRING_AGG(MG.Name, ', ' ), 'No Exercises logged') AS MusclesWorked ");
        sb.Append("FROM Workouts w ");
        sb.Append("LEFT JOIN (");
        sb.Append("SELECT DISTINCT mg.Name, e.WorkoutID ");
        sb.Append("FROM Exercises e ");
        sb.Append("JOIN ExerciseTypes et ON e.ExerciseTypeID = et.ID ");
        sb.Append("JOIN MuscleGroups mg ON et.MuscleGroupID = mg.ID) MG ON MG.WorkoutID = w.ID ");
        sb.Append("WHERE w.UserID = ");
        sb.Append(App.USER_ID);
        sb.Append(" GROUP BY w.CreatedOn, w.ID");

        await SqlCommunicator.Select(new SqlCommand(sb.ToString()), reader =>
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
        });
    }

    public async Task<int> InsertWorkoutAsync()
    {
        SqlCommand command = new("INSERT INTO Workouts (CreatedOn, UserID) VALUES(@date, @userID)");
        command.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));
        command.Parameters.AddWithValue("@userID", App.USER_ID);
        
        int workoutID = await SqlCommunicator.Insert(command, "could not create workout");
        return workoutID;
    }
}