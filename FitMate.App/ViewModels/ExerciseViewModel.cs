using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Data.SqlClient;

namespace FitMate.ViewModels;

public partial class ExerciseViewModel : ObservableObject, IQueryAttributable
{
    public ObservableCollection<Models.Exercise> Exercises { get; set; } = [];
    public event Action<string> UpdateTitleEvent; 

    private int ExerciseTypeID { get; set; } = -1;
    private int WorkoutID { get; set; } = -1;

    [ObservableProperty]
    private string? kgsOrMtr;
    [ObservableProperty]
    private string? repsOrSecs;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("exercise_type_id", out object? value)) { ExerciseTypeID = (int)value; }

        if (query.TryGetValue("workout_id", out value)) { WorkoutID = (int)value; }

        if (WorkoutID < 0 || ExerciseTypeID < 0)
        {
            throw new Exception(
                $"Both {nameof(WorkoutID)} and {nameof(ExerciseTypeID)} are expected to be initalized!");
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
            if (!reader.HasRows)
            {
                connection.Close();
                return;
            }

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

        connection.Close();
        
        UpdateTitleEvent.Invoke(Exercises.Count >= 1 ? Exercises[0].ExerciseType.Name : "Unknown exercise");
    }

    private string GenerateExercisesQuery() =>
        "SELECT e.KgsOrMtr, e.RepsOrSecs, e.IsPR, et.Name, et.MeasurementTypeID " +
        "FROM Exercise e JOIN Workouts w ON e.WorkoutID = w.ID " +
        "JOIN ExercisesTypes et ON e.ExerciseTypeID = et.ID " +
        $"WHERE w.ID = {WorkoutID} AND et.ID = {ExerciseTypeID}";
}