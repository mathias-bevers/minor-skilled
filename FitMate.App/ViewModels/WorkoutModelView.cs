using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using CommunityToolkit.Mvvm.ComponentModel;
using FitMate.Utils;
using Microsoft.Data.SqlClient;

namespace FitMate.ViewModels;

public partial class WorkoutModelView : ObservableObject, IQueryAttributable
{
    public int WorkoutID { get; private set; } = -1;
    public ObservableCollection<ExerciseGroup> Exercises { get; set; } = [];

    [ObservableProperty]
    public string emptyWorkoutMessage = "Loading ...";

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out object? value))
        {
            WorkoutID = (int)value;
        }

        if (WorkoutID < 0)
        {
            throw new InvalidOperationException("Workout ID needs to be set at least once!");
        }

        LoadWorkout();
    }

    private void LoadWorkout()
    {
        Exercises.Clear();

        List<Models.Exercise> exercises = [];

        EmptyWorkoutMessage = "Loading ...";

        SqlCommand command = new("SELECT e.KgsOrMtr, e.RepsOrSecs, et.ID, et.Name, et.MeasurementTypeID " +
                                 "FROM Exercises e " +
                                 "JOIN ExerciseTypes et ON e.ExerciseTypeID = et.ID " +
                                 "WHERE WorkoutID = @wID");
        command.Parameters.AddWithValue("@wID", WorkoutID);

        Task<bool> task = Task.Run(() => SqlCommunicator.Select(command, reader =>
        {
            exercises.Add(new Models.Exercise
            {
                KgsOrMtr = Convert.ToInt32(reader["KgsOrMtr"]),
                RepsOrSecs = Convert.ToInt32(reader["RepsOrSecs"]),
                ExerciseType = new Models.ExerciseType
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    Name = Convert.ToString(reader["Name"]) ??
                           throw new SqlNullValueException("The ExerciseType.Name is null!"),
                    MeasurementTypeID = Convert.ToInt32(reader["MeasurementTypeID"])
                }
            });
        }));

        if (task.Result)
        {
            foreach (ExerciseGroup group in exercises.GroupBy(e => e.ExerciseType.Name)
                         .Select(g => new ExerciseGroup(g.Key.ToString(), g.ToList())))
            {
                Exercises.Add(group);
            }
        }
        else
        {
            EmptyWorkoutMessage = "no exercises logged...";
        }
    }
}