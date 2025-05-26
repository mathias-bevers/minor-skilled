using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using CommunityToolkit.Mvvm.ComponentModel;
using FitMate.Models;
using Microsoft.Data.SqlClient;

namespace FitMate.ViewModels;

public partial class ExerciseViewModel : ObservableObject, IQueryAttributable
{
    public event Action<string> UpdateTitleEvent;
    public int ExerciseTypeID { get; private set; } = -1;
    public ObservableCollection<Exercise> Exercises { get; set; } = [];
    public TimeSpan Seconds { get; set; }
    private int WorkoutID { get; set; } = -1;

    [ObservableProperty]
    private bool isInKgs;
    [ObservableProperty]
    private bool isTimePickerOpened;

    private string exerciseTypeName = null!;
    [ObservableProperty]
    private string? kgsOrMtr;
    [ObservableProperty]
    private string? repetitions;
    [ObservableProperty]
    private string? timeButton = "Set Time";


    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("exercise_type_id", out object? etID)) { ExerciseTypeID = Convert.ToInt32(etID); }

        if (query.TryGetValue("exercise_type_name", out object? etName))
        {
            exerciseTypeName = Convert.ToString(etName) ?? "name not found";
        }

        if (query.TryGetValue("workout_id", out object? woID)) { WorkoutID = Convert.ToInt32(woID); }


        if (WorkoutID < 0 || ExerciseTypeID <= 0)
        {
            throw new Exception(
                $"Both {nameof(WorkoutID)} and {nameof(ExerciseTypeID)} are expected to be initialized!");
        }

        LoadFromDb();
    }

    private void LoadFromDb()
    {
        Exercises.Clear();
        Task.Run(LoadExercisesAsync);
    }

    private async Task LoadExercisesAsync()
    {
        UpdateTitleEvent.Invoke(exerciseTypeName);

        await using SqlConnection connection = new(App.SETTINGS.Server.ConnectionString);
        connection.Open();

        await using SqlCommand command = new(GetExercisesQuery(), connection);

        SqlDataReader reader = await command.ExecuteReaderAsync();

        if (!reader.HasRows)
        {
            reader.Close();

            command.CommandText = "SELECT mt.ID FROM MeasurementTypes mt " +
                                  $"JOIN ExerciseTypes et ON mt.ID = et.MeasurementTypeID WHERE et.ID = {ExerciseTypeID}";

            reader = await command.ExecuteReaderAsync();

            int measurementID = -1;
            while (reader.Read()) { measurementID = Convert.ToInt32(reader["ID"]); }

            IsInKgs = measurementID == 1;

            reader.Close();
            connection.Close();
            return;
        }

        while (reader.Read())
        {
            Exercises.Add(new Exercise
            {
                KgsOrMtr = Convert.ToInt32(reader["KgsOrMtr"]),
                RepsOrSecs = Convert.ToInt32(reader["RepsOrSecs"]),
                IsPR = Convert.ToBoolean(reader["IsPR"]),
                ExerciseType = new ExerciseType
                {
                    Name = Convert.ToString(reader["Name"]) ?? throw new SqlNullValueException(),
                    MeasurementTypeID = Convert.ToInt32(reader["MeasurementTypeID"])
                }
            });
        }

        IsInKgs = Exercises[0].ExerciseType.MeasurementType == Measurement.KilosPerRepetition;

        reader.Close();
        connection.Close();
    }

    public async Task<string?> AddExerciseAsync(int kgsOrMtr, int repsOrSecs)
    {
        bool isPR;
        try { isPR = await DetermineIfNewPR(kgsOrMtr, repsOrSecs); }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine(e.Message);
            return "Could not determine if the exercise is a new pr, try again.";
        }

        await using SqlConnection connection = new(App.SETTINGS.Server.ConnectionString);
        await using SqlCommand command = new();
        command.Connection = connection;
        command.CommandText = "INSERT INTO Exercises(KgsOrMtr, RepsOrSecs, IsPR, WorkoutID, ExerciseTypeID) " +
                              $"VALUES({kgsOrMtr}, {repsOrSecs}, {Convert.ToInt32(isPR)}, {WorkoutID}, {ExerciseTypeID})";

        try
        {
            connection.Open();
            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine(e.Message, "ERROR");
            return "Something went wrong during saving, try to add again.";
        }

        command.CommandText = GetExercisesQuery();

        try
        {
            Exercises.Clear();
            SqlDataReader reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Exercises.Add(new Exercise
                    {
                        KgsOrMtr = Convert.ToInt32(reader["KgsOrMtr"]),
                        RepsOrSecs = Convert.ToInt32(reader["RepsOrSecs"]),
                        IsPR = Convert.ToBoolean(reader["IsPR"]),
                        ExerciseType = new ExerciseType
                        {
                            Name = Convert.ToString(reader["Name"]) ?? throw new SqlNullValueException(),
                            MeasurementTypeID = Convert.ToInt32(reader["MeasurementTypeID"])
                        }
                    });
                }
            }
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine(e.Message, "ERROR");
            return "Something went wrong while updating the list, reload the page to see results.";
        }


        connection.Close();

        return null;
    }

    private async Task<bool> DetermineIfNewPR(int kgsOrMtr, int repsOrSecs)
    {
        await using SqlConnection connection = new(App.SETTINGS.Server.ConnectionString);
        await using SqlCommand command = new();
        command.Connection = connection;

        command.CommandText = "SELECT e.ID, e.KgsOrMtr, e.RepsOrSecs, et.MeasurementTypeID FROM Exercises e " +
                              "JOIN Workouts w ON e.WorkoutID = w.ID JOIN Users u ON u.ID = w.UserID " +
                              $"JOIN ExerciseTypes et ON e.ExerciseTypeID = et.ID WHERE u.ID = {App.USER_ID} " +
                              $"AND e.IsPR = 1 AND et.ID = {ExerciseTypeID};";

        connection.Open();

        Exercise? pr = null;
        Measurement? prMeasurementType = null;

        SqlDataReader reader = await command.ExecuteReaderAsync();

        if (!reader.HasRows)
        {
            connection.Close();
            return true;
        }

        while (reader.Read())
        {
            prMeasurementType = (Measurement)(Convert.ToInt32(reader["MeasurementTypeID"]) - 1);

            pr = new Exercise();
            pr.ID = Convert.ToInt32(reader["ID"]);
            pr.KgsOrMtr = Convert.ToInt32(reader["KgsOrMtr"]);
            pr.RepsOrSecs = Convert.ToInt32(reader["RepsOrSecs"]);
        }

        reader.Close();

        if (!prMeasurementType.HasValue || pr is null)
        {
            connection.Close();
            throw new SqlNullValueException();
        }

        bool isNewPR = prMeasurementType == Measurement.KilosPerRepetition
            ? repsOrSecs * kgsOrMtr > pr.RepsOrSecs * pr.KgsOrMtr
            : repsOrSecs * 1000.0 / kgsOrMtr < pr.RepsOrSecs * 1000.0 / pr.KgsOrMtr;

        if (!isNewPR)
        {
            connection.Close();
            return false;
        }

        command.CommandText = $"UPDATE Exercises SET IsPR = 0 WHERE ID = {pr.ID}";
        command.Parameters.AddWithValue("@eID", pr.ID);

        command.ExecuteNonQuery();

        connection.Close();
        return true;
    }

    private string GetExercisesQuery() =>
        "SELECT e.KgsOrMtr, e.RepsOrSecs, e.IsPR, et.Name, et.MeasurementTypeID " +
        "FROM Exercises e JOIN Workouts w ON e.WorkoutID = w.ID " +
        "JOIN ExerciseTypes et ON e.ExerciseTypeID = et.ID " + $"WHERE w.ID = {WorkoutID} AND et.ID = {ExerciseTypeID}";
}