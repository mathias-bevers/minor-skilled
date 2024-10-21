using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FitMate.Models;
using Microsoft.Data.SqlClient;

namespace FitMate.ViewModels;

public partial class ExerciseHistoryViewModel : ObservableObject, IQueryAttributable
{
    public event Action<string> UpdateTitleEvent;

    public ObservableCollection<ExerciseGroup> Exercises { get; set; } = [];

    [ObservableProperty]
    private Exercise personalRecord = null!;
    private string exerciseName = string.Empty;


    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("exercise_name", out object? value))
        {
            exerciseName = Convert.ToString(value) ?? string.Empty;
        }

        if (string.IsNullOrEmpty(exerciseName)) { throw new Exception("'exerciseName' cannot be null or empty!"); }

        Task.Run(LoadHistoryFromDB);
        UpdateTitleEvent?.Invoke($"{exerciseName}'s History");
    }

    public async Task LoadHistoryFromDB()
    {
        List<Exercise> exercises = [];
        await using SqlConnection connection = new(App.SETTINGS.Server.ConnectionString);
        await using SqlCommand command = new(GenerateHistoryQuery(), connection);

        try
        {
            connection.Open();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (!reader.HasRows)
            {
                connection.Close();
                return;
            }

            while (reader.Read())
            {
                Exercise exercise = new()
                {
                    KgsOrMtr = (int)reader["KgsOrMtr"],
                    RepsOrSecs = (int)reader["RepsOrSecs"],
                    IsPR = (bool)reader["IsPR"],
                    Date = DateTime.Parse((string)reader["CreatedOn"]),
                    ExerciseType = new ExerciseType
                    {
                        Name = (string)reader["Name"],
                        MeasurementTypeID = (int)reader["MeasurementTypeID"]
                    }
                };

                exercises.Add(exercise);

                if (!exercise.IsPR) { continue; }

                PersonalRecord = exercise;
            }
        }
        catch (Exception e) { System.Diagnostics.Debug.WriteLine(e); }
        
        connection.Close();
        
        foreach (ExerciseGroup exerciseGroup in exercises.GroupBy(e => e.Date)
                     .Select(g => new ExerciseGroup(g.Key.ToString("dddd - dd/MM/yyyy"), g.ToList())))
        {
            Exercises.Add(exerciseGroup);
        }
    }

    private string GenerateHistoryQuery() =>
        "SELECT e.KgsOrMtr, e.RepsOrSecs, e.IsPR, et.Name, w.CreatedOn, et.MeasurementTypeID FROM Exercises e " +
        "JOIN ExerciseTypes et ON e.ExerciseTypeName  = et.Name JOIN Workouts w ON e.WorkoutID = w.ID " +
        $"JOIN Users u ON u.ID = {App.USER_ID} WHERE et.Name = \'{exerciseName}\';";
}