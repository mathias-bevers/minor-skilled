using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FitMate.Models;
using Microsoft.Data.SqlClient;

namespace FitMate.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
    public ObservableCollection<Exercise> PersonalRecords { get; set; } = [];

    [ObservableProperty]
    private User user = new();

    public void LoadFromDbAsync()
    {
        PersonalRecords.Clear();
        Task.Run(LoadUserAsync);
        Task.Run(LoadPersonalRecordsAsync);
    }


    private async Task LoadPersonalRecordsAsync()
    {
        List<Exercise> loadedList = [];

        System.Diagnostics.Debug.WriteLine("start pr");

        await using SqlConnection connection = new(App.SERVER_SETTINGS.ConnectionString);
        connection.Open();

        await using (SqlCommand command = new(GetPersonalRecordsQuery(), connection))
        {
            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int kgsOrMtr = (int)reader["KgsOrMtr"];
                    int repsOrSecs = (int)reader["RepsOrSecs"];
                    string name = reader["Name"].ToString() ?? "ERROR";
                    int measurement = (int)reader["MeasurementTypeID"];

                    Exercise exercise = new()
                    {
                        KgsOrMtr = kgsOrMtr,
                        RepsOrSecs = repsOrSecs,
                        ExerciseType = new ExerciseType
                        {
                            Name = name,
                            MeasurementTypeID = measurement
                        }
                    };
                    loadedList.Add(exercise);
                }
            }
        }

        connection.Close();

        foreach (Exercise e in loadedList) { PersonalRecords.Add(e); }
        System.Diagnostics.Debug.WriteLine("end pr");
    }

    private async Task LoadUserAsync()
    {
        System.Diagnostics.Debug.WriteLine("start user");
        await using SqlConnection connection = new(App.SERVER_SETTINGS.ConnectionString);

        connection.Open();

        await using (SqlCommand command = new(GetUserQuery(), connection))
        {
            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    User = new User
                    {
                        UserName = reader["UserName"].ToString() ?? "ERROR",
                        GenderID = (int)reader["GenderID"],
                        DateOfBirth = reader["DateOfBirth"].ToString() ?? "ERROR"
                    };
                }
            }
        }

        connection.Close();
        System.Diagnostics.Debug.WriteLine("end user");
    }

    private static string GetUserQuery() =>
        $"SELECT u.UserName, u.GenderID, u.DateOfBirth FROM Users u WHERE ID = {App.USER_ID}";

    private static string GetPersonalRecordsQuery() =>
        "SELECT e.KgsOrMtr, e.RepsOrSecs, et.Name, et.MeasurementTypeID " +
        "FROM Exercise e JOIN Workouts w ON e.WorkoutID  = w.ID " +
        "JOIN Users u ON u.ID = w.UserID JOIN ExercisesTypes et ON " +
        $"e.ExerciseTypeID = et.ID WHERE u.ID = {App.USER_ID} AND e.IsPR = 1;";
}