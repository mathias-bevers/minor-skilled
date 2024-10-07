using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FitMate.Models;
using Microsoft.Data.SqlClient;

namespace FitMate.ViewModels;

public partial class ExerciseHistoryViewModel : ObservableObject
{
    public ObservableCollection<ExerciseGroup> Exercises { get; set; } = [];
    [ObservableProperty]
    private Exercise personalRecord = null!;
    private int exerciseTypeID = 1;

    public void LoadHistoryFromDB()
    {
        List<Exercise> exercises = [];
        using (SqlConnection connection = new(App.SERVER_SETTINGS.ConnectionString))
        {
            connection.Open();
            using (SqlCommand command = new(GenerateHistoryQuery(), connection))
            {
                SqlDataReader reader = command.ExecuteReader();

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

            connection.Close();
        }

        foreach (ExerciseGroup exerciseGroup in exercises.GroupBy(e => e.Date)
                     .Select(g => new ExerciseGroup(g.Key.ToString("dddd - dd/MM/yyyy"), g.ToList())))
        {
            Exercises.Add(exerciseGroup);
        }
    }

    //TODO: Load correct exercise!
    private string GenerateHistoryQuery() =>
        "SELECT e.KgsOrMtr, e.RepsOrSecs, e.IsPR, et.Name, w.CreatedOn, et.MeasurementTypeID FROM Exercise e " +
        "JOIN ExercisesTypes et ON e.ExerciseTypeID  = et.ID JOIN Workouts w ON e.WorkoutID = w.ID " +
        $"JOIN Users u ON u.ID = {App.USER_ID} WHERE et.ID = {exerciseTypeID};";

    private string GeneratePresonalRecordQuery() =>
        "SELECT e.KgsOrMtr, e.RepsOrSecs, e.IsPR, et.Name FROM Exercise e " +
        "JOIN ExercisesTypes et ON e.ExerciseTypeID  = et.ID JOIN Workouts w ON e.WorkoutID = w.ID " +
        $"JOIN Users u ON u.ID = {App.USER_ID} WHERE et.ID = {exerciseTypeID} AND e.IsPR = 1;";
}