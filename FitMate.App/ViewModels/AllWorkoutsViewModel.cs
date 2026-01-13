using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using CommunityToolkit.Mvvm.ComponentModel;
using FitMate.Models;
using FitMate.Utils;
using Microsoft.Data.SqlClient;
using Nito.AsyncEx.Synchronous;

namespace FitMate.ViewModels;

public partial class AllWorkoutsViewModel : ObservableObject
{
    public ObservableCollection<Workout> Workouts { get; set; } = [];

    [ObservableProperty]
    private bool canAddNew = false;

    public void OnAppearing()
    {
        SelectFromDB();
    }

    private void SelectFromDB()
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

        Task.Run(() => SqlCommunicator.Select(new SqlCommand(sb.ToString()), reader =>
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
        })).WaitAndUnwrapException();

        CanAddNew = true;
    }

    public static bool HasWorkoutForToday()
    {
        string dateString = DateTime.Now.ToString("yyyy-MM-dd");
        SqlCommand select = new("SELECT TOP 1 id FROM Workouts w WHERE w.UserID = @user_id and w.CreatedOn = @date");
        select.Parameters.AddWithValue("@date", dateString);
        select.Parameters.AddWithValue("@user_id", App.USER_ID);

        bool exists = Task.Run(() => SqlCommunicator.Select(select, _ => { })).WaitAndUnwrapException();
        return exists;
    }

    
    public static int InsertWorkout(bool force = false)
    {
        string dateString = DateTime.Now.ToString("yyyy-MM-dd");
        SqlCommand insert = new("INSERT INTO Workouts (CreatedOn, UserID) VALUES(@date, @user_id)");
        insert.Parameters.AddWithValue("@date", dateString);
        insert.Parameters.AddWithValue("@user_id", App.USER_ID);

        int workoutID = Task.Run(() => SqlCommunicator.Insert(insert, "could not create workout"))
            .WaitAndUnwrapException();
        return workoutID;
    }
}