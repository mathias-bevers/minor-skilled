using System.Collections.ObjectModel;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Primitives;

namespace FitMate.ViewModels;

public class WorkoutModelView : ObservableObject, IQueryAttributable
{
    public ObservableCollection<ExerciseGroup> Exercises { get; set; } = [];
    private int WorkoutID { get; set; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        WorkoutID = (int)query["id"];
        LoadFromDB();
    }

    public void LoadFromDB()
    {
        List<Models.Exercise> exercises = [];

        using SqlConnection connection = new(App.SERVER_SETTINGS.ConnectionString);

        connection.Open();

        //TODO: rename this to Exercises when the next migration is applied.
        using (SqlCommand command = new(GetQuery(), connection))
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
                    ExerciseType = new Models.ExerciseType
                    {
                        Name = reader["Name"].ToString() ?? "ERROR",
                        MeasurementTypeID = Convert.ToInt32(reader["MeasurementTypeID"])
                    }
                });
            }
        }

        connection.Close();

        foreach (ExerciseGroup group in exercises.GroupBy(e => e.ExerciseType.Name)
                     .Select(g => new ExerciseGroup(g.Key.ToString(), g.ToList()))) { Exercises.Add(group); }
    }

    public string GetQuery()
    {
        StringBuilder sb = new();
        
        sb.Append("SELECT ");
        sb.Append("e.KgsOrMtr, e.RepsOrSecs, e.IsPR,");
        sb.Append("et.Name, et.MeasurementTypeID ");
        sb.Append("FROM Exercise e JOIN ExercisesTypes et ");
        sb.Append("ON e.ExerciseTypeID = et.ID ");
        sb.Append("WHERE WorkoutID = ");
        sb.Append(WorkoutID);

        return sb.ToString();
    }
}