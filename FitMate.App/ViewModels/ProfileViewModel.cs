using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using CommunityToolkit.Mvvm.ComponentModel;
using FitMate.Models;
using FitMate.Utils;
using Microsoft.Data.SqlClient;

namespace FitMate.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
    public ObservableCollection<Exercise> PersonalRecords { get; set; } = [];

    [ObservableProperty]
    private User user = new();

    public void LoadDataFromDB()
    {
        SelectUser();
        SelectPersonalRecords();
    }


    private void SelectPersonalRecords()
    {
        PersonalRecords.Clear();
        
        Exercise[] prs = PersonalRecordFinder.FindAll();
        for (int i = 0; i < prs.Length; ++i)
        {
            PersonalRecords.Add(prs[i]);
        }
    }

    private async Task SelectUser()
    {
        await using SqlConnection connection = new(App.SETTINGS.Server.ConnectionString);

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
}