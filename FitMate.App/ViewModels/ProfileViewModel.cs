using System.Collections.ObjectModel;
using System.Data.SqlTypes;
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
        Task.Run(LoadUserAsync);
        Task.Run(LoadPersonalRecordsAsync);
    }


    private async Task LoadPersonalRecordsAsync()
    {
        PersonalRecords.Clear();

        await using SqlConnection connection = new(App.SERVER_SETTINGS.ConnectionString);
        connection.Open();

        await using (SqlCommand command = new(GetPersonalRecordsQuery(), connection))
        {
            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    PersonalRecords.Add(new Exercise
                    {
                        KgsOrMtr = Convert.ToInt32(reader["KgsOrMtr"]),
                        RepsOrSecs = Convert.ToInt32(reader["RepsOrSecs"]),
                        ExerciseType = new ExerciseType
                        {
                            Name = Convert.ToString(reader["Name"]) ??
                                   throw new SqlNullValueException("reader[\"Name\"]"),
                            MeasurementTypeID = Convert.ToInt32(reader["MeasurementTypeID"])
                        }
                    });
                }
            }
        }

        connection.Close();
    }

    private async Task LoadUserAsync()
    {
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
    }

    private static string GetUserQuery() =>
        $"SELECT u.UserName, u.GenderID, u.DateOfBirth FROM Users u WHERE ID = {App.USER_ID}";

    private static string GetPersonalRecordsQuery() =>
        "SELECT e.KgsOrMtr, e.RepsOrSecs, et.Name, et.MeasurementTypeID " +
        "FROM Exercise e JOIN Workouts w ON e.WorkoutID  = w.ID " +
        "JOIN Users u ON u.ID = w.UserID JOIN ExercisesTypes et ON " +
        $"e.ExerciseTypeID = et.ID WHERE u.ID = {App.USER_ID} AND e.IsPR = 1;";
}