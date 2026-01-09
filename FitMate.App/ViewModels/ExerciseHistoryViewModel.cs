using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using CommunityToolkit.Mvvm.ComponentModel;
using FitMate.Models;
using FitMate.Utils;
using Microsoft.Data.SqlClient;
using Nito.AsyncEx.Synchronous;

namespace FitMate.ViewModels;

public partial class ExerciseHistoryViewModel : ObservableObject, IQueryAttributable
{
    public event Action<string> UpdateTitleEvent;

    public ObservableCollection<ExerciseGroup> Exercises { get; set; } = [];

    [ObservableProperty]
    private Exercise personalRecord = null!;
    private int exerciseID = -1;
    private string exerciseName = string.Empty;


    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("exercise_id", out object? value))
        {
            exerciseID = Convert.ToInt32(value);
        }

        if (query.TryGetValue("exercise_name", out value))
        {
            exerciseName = Convert.ToString(value) ?? string.Empty;
        }

        if (string.IsNullOrEmpty(exerciseName))
        {
            throw new Exception("'exerciseName' cannot be null or empty!");
        }

        UpdateTitleEvent?.Invoke($"{exerciseName}'s History");
        SelectHistory();
    }

    private void SelectHistory()
    {
        List<Exercise> exercises = [];
        SqlCommand command = new("SELECT e.KgsOrMtr, e.RepsOrSecs, et.Name, w.CreatedOn, et.MeasurementTypeID " +
                                 "FROM Exercises e JOIN ExerciseTypes et ON e.ExerciseTypeID = et.ID " +
                                 "JOIN Workouts w ON e.WorkoutID = w.ID " + "JOIN Users u ON u.ID = @uID " +
                                 "WHERE et.Name = @eName");
        command.Parameters.AddWithValue("@uID", App.USER_ID);
        command.Parameters.AddWithValue("@eName", exerciseName);

        Task.Run(() => SqlCommunicator.Select(command, reader =>
        {
            Exercise exercise = new()
            {
                KgsOrMtr = Convert.ToInt32(reader["KgsOrMtr"]),
                RepsOrSecs = Convert.ToInt32(reader["RepsOrSecs"]),
                Date = DateTime.Parse(Convert.ToString(reader["CreatedOn"]) ?? "null"),
                ExerciseType = new ExerciseType
                {
                    Name = Convert.ToString(reader["Name"]) ?? throw new SqlNullValueException("etName"),
                    MeasurementTypeID = Convert.ToInt32(reader["MeasurementTypeID"])
                }
            };

            exercises.Add(exercise);
        })).WaitAndUnwrapException();

        foreach (ExerciseGroup exerciseGroup in exercises.GroupBy(e => e.Date)
                     .Select(g => new ExerciseGroup(g.Key.ToString("dddd - dd/MM/yyyy"), g.ToList())))
        {
            Exercises.Add(exerciseGroup);
        }

        int prID = PersonalRecordFinder.FindForExerciseID(exerciseID);
        System.Diagnostics.Debug.WriteLine($"found pr for {exerciseName} is id: {prID}");
    }
}