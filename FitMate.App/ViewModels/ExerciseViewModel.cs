using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using CommunityToolkit.Mvvm.ComponentModel;
using FitMate.Models;
using Microsoft.Data.SqlClient;

namespace FitMate.ViewModels;

public partial class ExerciseViewModel : ObservableObject, IQueryAttributable
{
    public event Action<string> UpdateTitleEvent;

    public ObservableCollection<Exercise> Exercises { get; set; } = [];
    public string ExerciseTypeName { get; private set; } = string.Empty;
    private int WorkoutID { get; set; } = -1;
    public TimeSpan Seconds { get; set; }

    [ObservableProperty]
    private bool isInKgs;
    [ObservableProperty]
    private bool isTimePickerOpened;
    [ObservableProperty]
    private string? kgsOrMtr;
    [ObservableProperty]
    private string? repetitions;
    [ObservableProperty]
    private string? timeButton = "Set Time";
   
    

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("exercise_type_name", out object? typeName))
        {
            ExerciseTypeName = Convert.ToString(typeName) ?? string.Empty;
        }

        if (query.TryGetValue("workout_id", out object? woID)) { WorkoutID = Convert.ToInt32(woID); }


        if (WorkoutID < 0 || string.IsNullOrEmpty(ExerciseTypeName))
        {
            throw new Exception(
                $"Both {nameof(WorkoutID)} and {nameof(ExerciseTypeName)} are expected to be initialized!");
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
        UpdateTitleEvent.Invoke(ExerciseTypeName);

        await using SqlConnection connection = new(App.SETTINGS.Server.ConnectionString);
        connection.Open();

        await using SqlCommand command = new(GetExercisesQuery(), connection);

        SqlDataReader reader = await command.ExecuteReaderAsync();

        if (!reader.HasRows)
        {
            reader.Close();

            command.CommandText = "SELECT ID FROM MeasurementTypes mt " +
                                  "JOIN ExerciseTypes et ON ID = et.MeasurementTypeID " + "WHERE et.Name = @etName";
            command.Parameters.AddWithValue("@etName", ExerciseTypeName);

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

        command.CommandText = "INSERT INTO Exercises (KgsOrMtr, RepsOrSecs, IsPR, WorkoutID, ExerciseTypeName) " +
                              "VALUES (@kgsOrMtr, @repsOrSecs, @isPR, @wID, @etName)";
        command.Parameters.AddWithValue("@kgsOrMtr", kgsOrMtr);
        command.Parameters.AddWithValue("@repsOrSecs", repsOrSecs);
        command.Parameters.AddWithValue("@isPR", isPR);
        command.Parameters.AddWithValue("@wID", WorkoutID);
        command.Parameters.AddWithValue("@etName", ExerciseTypeName);

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
                              "JOIN ExerciseTypes et ON e.ExerciseTypeName = et.Name WHERE u.ID = @uID " +
                              "AND e.IsPR = 1 AND et.Name = @etName;";
        command.Parameters.AddWithValue("@uID", App.USER_ID);
        command.Parameters.AddWithValue("@etName", ExerciseTypeName);

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

        command.CommandText = "UPDATE Exercises SET IsPR = 0 WHERE ID = @eID";
        command.Parameters.AddWithValue("@eID", pr.ID);

        command.ExecuteNonQuery();

        connection.Close();
        return true;
    }

    private string GetExercisesQuery() =>
        "SELECT e.KgsOrMtr, e.RepsOrSecs, e.IsPR, et.Name, et.MeasurementTypeID " +
        "FROM Exercises e JOIN Workouts w ON e.WorkoutID = w.ID " +
        "JOIN ExerciseTypes et ON e.ExerciseTypeName = et.Name " +
        $"WHERE w.ID = {WorkoutID} AND et.Name = '{ExerciseTypeName}'";
}