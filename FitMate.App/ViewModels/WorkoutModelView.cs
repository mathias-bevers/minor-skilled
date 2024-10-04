using System.Collections.ObjectModel;
using System.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Data.SqlClient;

namespace FitMate.ViewModels;

public class WorkoutModelView : ObservableObject, IQueryAttributable
{
    public ObservableCollection<ExerciseGroup> Exercises { get; set; } = [];

    private int WorkoutID { get; set; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        WorkoutID = (int)query["id"];
        LoadFromDB(WorkoutID);
    }

    public void LoadFromDB(int workoutID)
    {
        List<Models.Exercise> exercises = [];

        using SqlConnection connection = new(App.SERVER_SETTINGS.ConnectionString);

        connection.Open();

        //TODO: rename this to Exercises when the next migration is applied.
        using (SqlCommand command = new($"SELECT * FROM Exercise WHERE WorkoutID = {workoutID}", connection))
        {
            SqlDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                connection.Close();
                return;
            }

            while (reader.Read())
            {
                exercises.Add(new Models.Exercise
                {
                    KgsOrMtr = Convert.ToInt32(reader["KgsOrMtr"]),
                    RepsOrSecs = Convert.ToInt32(reader["RepsOrSecs"]),
                    IsPR = (bool)reader["IsPR"],
                    ExerciseTypeID = Convert.ToInt32(reader["ExerciseTypeID"])
                });
            }
        }

        connection.Close();

        foreach (ExerciseGroup group in exercises.GroupBy(e => e.ExerciseTypeID)
                     .Select(g => new ExerciseGroup(g.Key.ToString(), g.ToList()))) { Exercises.Add(group); }
    }
}