using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using CommunityToolkit.Mvvm.ComponentModel;
using FitMate.Models;
using FitMate.Utils;
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
        sb.Append("COALESCE(STRING_AGG(MG.Name, '\\t' ), 'No Exercises logged') AS MusclesWorked ");
        sb.Append("FROM Workouts w ");
        sb.Append("LEFT JOIN (");
        sb.Append("SELECT DISTINCT mg.Name, e.WorkoutID ");
        sb.Append("FROM Exercises e ");
        sb.Append("JOIN ExerciseTypes et ON e.ExerciseTypeID = et.ID ");
        sb.Append("JOIN MuscleGroups mg ON et.MuscleGroupID = mg.ID) MG ON MG.WorkoutID = w.ID ");
        sb.Append("WHERE w.UserID = ");
        sb.Append(App.USER_ID);
        sb.Append(" GROUP BY w.CreatedOn, w.ID");

        await SqlCommunicator.Select(sb.ToString(), reader =>
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
        string dateString = DateTime.Now.ToString("yyyy-MM-dd");
        string query = $"INSERT INTO Workouts (CreatedOn, UserID) VALUES(\'{dateString}\', {App.USER_ID})";
        int workoutID = await SqlCommunicator.Insert(query, "could not create workout");
        return workoutID;
    }
}