using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Data.SqlClient;

namespace FitMate.ViewModels;

public partial class ExerciseViewModel : ObservableObject, IQueryAttributable
{
    public event Action<string> UpdateTitleEvent;

    public ObservableCollection<Models.Exercise> Exercises { get; set; } = [];
    public string ExerciseTypeName { get; private set; } = string.Empty;
    private int WorkoutID { get; set; } = -1;

    [ObservableProperty]
    private string? kgsOrMtr;
    [ObservableProperty]
    private string? repsOrSecs;

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
        await using SqlConnection connection = new(App.SERVER_SETTINGS.ConnectionString);
        connection.Open();

        await using (SqlCommand command = new(GenerateExercisesQuery(), connection))
        {
            SqlDataReader reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Exercises.Add(new Models.Exercise
                    {
                        KgsOrMtr = Convert.ToInt32(reader["KgsOrMtr"]),
                        RepsOrSecs = Convert.ToInt32(reader["RepsOrSecs"]),
                        IsPR = Convert.ToBoolean(reader["IsPR"]),
                        ExerciseType = new Models.ExerciseType
                        {
                            Name = Convert.ToString(reader["Name"]) ?? throw new SqlNullValueException(),
                            MeasurementTypeID = Convert.ToInt32(reader["MeasurementTypeID"])
                        }
                    });
                }
            }
        }

        connection.Close();

        UpdateTitleEvent.Invoke(ExerciseTypeName);
    }

    private string GenerateExercisesQuery() =>
        "SELECT e.KgsOrMtr, e.RepsOrSecs, e.IsPR, et.Name, et.MeasurementTypeID " +
        "FROM Exercises e JOIN Workouts w ON e.WorkoutID = w.ID " +
        "JOIN ExerciseTypes et ON e.ExerciseTypeName = et.Name " +
        $"WHERE w.ID = {WorkoutID} AND et.Name = '{ExerciseTypeName}'";
}