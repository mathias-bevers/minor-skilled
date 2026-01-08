using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using CommunityToolkit.Mvvm.ComponentModel;
using FitMate.Models;
using FitMate.Utils;
using Microsoft.Data.SqlClient;
using Nito.AsyncEx.Synchronous;

namespace FitMate.ViewModels;

public partial class ExerciseViewModel : ObservableObject, IQueryAttributable, IDisposable
{
    public event Action<string> UpdateTitleEvent;
    public int ExerciseTypeID { get; private set; } = -1;
    public ObservableCollection<Exercise> Exercises { get; set; } = [];

    public string ExerciseTypeName { get; private set; } = null!;
    public TimeSpan Seconds { get; set; }
    private int WorkoutID { get; set; } = -1;

    [ObservableProperty]
    private bool isInKgs;
    [ObservableProperty]
    private bool isTimePickerOpened;
    private CancellationTokenSource cts;
    [ObservableProperty]
    private string? kgsOrMtr;
    [ObservableProperty]
    private string? repetitions;
    [ObservableProperty]
    private string? timeButton = "Set Time";

    public void Dispose()
    {
        cts.Cancel();
    }


    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("exercise_type_id", out object? etID))
        {
            ExerciseTypeID = Convert.ToInt32(etID);
        }

        if (query.TryGetValue("exercise_type_name", out object? etName))
        {
            ExerciseTypeName = Convert.ToString(etName) ?? "name not found";
        }

        if (query.TryGetValue("workout_id", out object? woID))
        {
            WorkoutID = Convert.ToInt32(woID);
        }


        if (WorkoutID < 0 || ExerciseTypeID <= 0)
        {
            throw new Exception(
                $"Both {nameof(WorkoutID)} and {nameof(ExerciseTypeID)} are expected to be initialized!");
        }

        SelectExercises();
    }

    private void SelectExercises()
    {
        UpdateTitleEvent.Invoke(ExerciseTypeName);

        Exercises.Clear();

        SqlCommand command = new("SELECT e.KgsOrMtr, e.RepsOrSecs, et.Name, et.MeasurementTypeID " +
                                 "FROM Exercises e " + "JOIN Workouts w ON e.WorkoutID = w.ID " +
                                 "JOIN ExerciseTypes et ON e.ExerciseTypeID = et.ID " +
                                 "WHERE w.ID = @wID AND et.ID = @etID");
        command.Parameters.AddWithValue("@wID", WorkoutID);
        command.Parameters.AddWithValue("@etID", ExerciseTypeID);

        bool hasRows = Task.Run(() => SqlCommunicator.Select(command, reader =>
        {
            Exercises.Add(new Exercise
            {
                KgsOrMtr = Convert.ToInt32(reader["KgsOrMtr"]),
                RepsOrSecs = Convert.ToInt32(reader["RepsOrSecs"]),
                ExerciseType = new ExerciseType
                {
                    Name = Convert.ToString(reader["Name"]) ?? throw new SqlNullValueException(),
                    MeasurementTypeID = Convert.ToInt32(reader["MeasurementTypeID"])
                }
            });
        })).WaitAndUnwrapException();

        if (!hasRows)
        {
            command = new SqlCommand("SELECT mt.ID FROM MeasurementTypes mt " +
                                     "JOIN ExerciseTypes et ON mt.ID = et.MeasurementTypeID WHERE et.ID = @etID");
            command.Parameters.AddWithValue("@etID", ExerciseTypeID);

            int measurementID = -1;
            Task.Run(() => SqlCommunicator.Select(command, reader => { measurementID = Convert.ToInt32(reader["ID"]); })
                .WaitAndUnwrapException());
            IsInKgs = measurementID == 1;
        }
        else
        {
            IsInKgs = Exercises[0].ExerciseType.MeasurementType == Measurement.KilosPerRepetition;
        }
    }

    public void InsertExercise(int kgsOrMtr, int repsOrSecs)
    {
        SqlCommand command = new("INSERT INTO Exercises(KgsOrMtr, RepsOrSecs, WorkoutID, ExerciseTypeID) " +
                                 "VALUES(@kom, @ros, @wID, @etID)");
        command.Parameters.AddWithValue("@kom", kgsOrMtr);
        command.Parameters.AddWithValue("@ros", repsOrSecs);
        command.Parameters.AddWithValue("@wID", WorkoutID);
        command.Parameters.AddWithValue("@etID", ExerciseTypeID);

        Task.Run(() => SqlCommunicator.Insert(command, "Something went wrong while inserting the exercise"))
            .WaitAndUnwrapException();
        SelectExercises();
    }
}