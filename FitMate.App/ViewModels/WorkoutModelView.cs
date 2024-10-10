using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Data.SqlClient;

namespace FitMate.ViewModels;

public partial class WorkoutModelView : ObservableObject, IQueryAttributable
{
    public ObservableCollection<ExerciseGroup> Exercises { get; set; } = [];
    public int WorkoutID { get; private set; } = -1;
    
    [ObservableProperty]
    public string emptyWorkoutMessage = "Loading ...";

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out object? value)) { WorkoutID = (int)value; }
        
        //TODO: add create workout tab.

        if (WorkoutID < 0) { throw new InvalidOperationException("Workout ID needs to be set at least once!"); }

        Exercises.Clear();
        Task.Run(LoadWorkoutAsync);
    }

    public async Task LoadWorkoutAsync()
    {
        List<Models.Exercise> exercises = [];
        EmptyWorkoutMessage = "Loading ...";

        await using SqlConnection connection = new(App.SERVER_SETTINGS.ConnectionString);

        connection.Open();
        
        await using (SqlCommand command = new(GenerateLoadWorkoutQuery(), connection))
        {
            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (!reader.HasRows)
            {
                EmptyWorkoutMessage = "No exercises logged!";
                connection.Close();
                return;
            }

            while (reader.Read())
            {
                exercises.Add(new Models.Exercise
                {
                    KgsOrMtr = Convert.ToInt32(reader["KgsOrMtr"]),
                    RepsOrSecs = Convert.ToInt32(reader["RepsOrSecs"]),
                    IsPR = Convert.ToBoolean(reader["IsPR"]),
                    ExerciseType = new Models.ExerciseType
                    {
                        Name = Convert.ToString(reader["Name"]) ??
                               throw new SqlNullValueException("The ExerciseType.Name is null!"),
                        MeasurementTypeID = Convert.ToInt32(reader["MeasurementTypeID"])
                    }
                });
            }
        }

        connection.Close();

        foreach (ExerciseGroup group in exercises.GroupBy(e => e.ExerciseType.Name)
                     .Select(g => new ExerciseGroup(g.Key.ToString(), g.ToList()))) { Exercises.Add(group); }
    }

    private string GenerateLoadWorkoutQuery() =>
        "SELECT e.KgsOrMtr, e.RepsOrSecs, e.IsPR, et.Name, et.MeasurementTypeID " +
        "FROM Exercises e JOIN ExerciseTypes et ON e.ExerciseTypeName = et.Name WHERE WorkoutID = " + WorkoutID;
}