using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FitMate.Models;
using FitMate.ViewModels.Mockups;
using Microsoft.Data.SqlClient;

namespace FitMate.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
    public ObservableCollection<PersonalRecordMockup> PlaceholderPRs { get; set; }

    [ObservableProperty]
    private User user = new();
    
    public ProfileViewModel()
    {
        PlaceholderPRs = new ObservableCollection<PersonalRecordMockup>([
            new PersonalRecordMockup
            {
                Name = "Bicep Curl",
                ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 14, 12)
            },
            new PersonalRecordMockup
            {
                Name = "Treadmill",
                ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.MeterMinutes, 1320, 422)
            },
            new PersonalRecordMockup
            {
                Name = "Leg Press",
                ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 160, 10)
            }
        ]);
    }

    public void LoadUserFromDB()
    {
        User tmp = new();
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

            connection.Close();
        }
        
        User = tmp;
    }

    private string GetUserQuery() =>
        $"SELECT u.UserName, u.GenderID, u.DateOfBirth FROM Users u WHERE ID = {App.USER_ID}";

    private string GetPersonalRecordsQuery()
    {
        
    }
}