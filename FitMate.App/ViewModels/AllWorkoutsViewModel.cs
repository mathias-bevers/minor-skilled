using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FitMate.Models;
using Microsoft.Data.SqlClient;

namespace FitMate.ViewModels;

public class AllWorkoutsViewModel : ObservableObject
{
    public ObservableCollection<Workout> Workouts { get; set; }
    
    public AllWorkoutsViewModel()
    {
        Workouts = [];

        using SqlConnection connection = new(App.SERVER_SETTINGS.ConnectionString);
        
        connection.Open();

        using (SqlCommand command = new($"SELECT * FROM Workouts WHERE UserID = {App.USER_ID}", connection))
        {
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Workouts.Add(new Workout()
                    {
                        CreatedOn = reader["CreatedOn"].ToString() ?? "error",
                        ID = (int)reader["ID"],
                    });
                }
            }
        }

        connection.Close();
    }
}