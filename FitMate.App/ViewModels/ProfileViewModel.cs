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

    public void LoadUserFromDB()
    {
        User tmp = new();
        List<Exercise> tmpList = [];
        PersonalRecords.Clear();

        using (SqlConnection connection = new(App.SERVER_SETTINGS.ConnectionString))
        {
            connection.Open();


            using (SqlCommand command = new(GetUserQuery(), connection))
            {
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tmp.UserName = reader["UserName"].ToString() ?? "ERROR";
                        tmp.GenderID = (int)reader["GenderID"];
                        tmp.DateOfBirth = reader["DateOfBirth"].ToString() ?? "ERROR";
                    }
                }
            }

            using (SqlCommand command = new(GetPersonalRecordsQuery(), connection))
            {
                SqlDataReader reader = command.ExecuteReader();

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
                        tmpList.Add(exercise);
                    }
                }
            }

            connection.Close();
        }
        
        User = tmp;

        for (int i = 0; i < tmpList.Count; ++i)
        {
            PersonalRecords.Add(tmpList[i]);
        }
    }

    private static string GetUserQuery() =>
        $"SELECT u.UserName, u.GenderID, u.DateOfBirth FROM Users u WHERE ID = {App.USER_ID}";

    private static string GetPersonalRecordsQuery() =>
        "SELECT e.KgsOrMtr, e.RepsOrSecs, et.Name, et.MeasurementTypeID " +
        "FROM Exercise e JOIN Workouts w ON e.WorkoutID  = w.ID " +
        "JOIN Users u ON u.ID = w.UserID JOIN ExercisesTypes et ON " +
        $"e.ExerciseTypeID = et.ID WHERE u.ID = {App.USER_ID} AND e.IsPR = 1;";
}