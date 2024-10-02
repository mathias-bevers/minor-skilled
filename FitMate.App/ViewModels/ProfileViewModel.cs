using System.Collections.ObjectModel;
using FitMate.Models;
using FitMate.ViewModels.Mockups;
using Microsoft.Data.SqlClient;

namespace FitMate.ViewModels;

public class ProfileViewModel
{
    public User User { get; }
    public int Age { get; }

    public ObservableCollection<PersonalRecordMockup> PlaceholderPRs { get; set; }

    public ProfileViewModel()
    {
        User = new User();

        using (SqlConnection connection = new(App.SERVER_SETTINGS.ConnectionString))
        {
            connection.Open();

            using (SqlCommand command = new("select top 1 * from Users", connection))
            {
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        User.UserName = reader["UserName"].ToString() ?? "ERROR";
                        User.GenderID = (int)reader["GenderID"];
                        User.DateOfBirth = reader["DateOfBirth"].ToString() ?? "ERROR";
                    }
                }
            }

            connection.Close();
        }

        DateTime dob = DateTime.ParseExact(User.DateOfBirth, "yyyy-MM-dd",
            System.Globalization.CultureInfo.InvariantCulture);

        Age = DateTime.Today.Year - dob.Year;
        if (DateTime.Today.DayOfYear < dob.DayOfYear) { --Age; }

        PlaceholderPRs = new ObservableCollection<PersonalRecordMockup>([
            new PersonalRecordMockup()
            {
                Name = "Bicep Curl",
                ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 14, 12),
            },
            new PersonalRecordMockup()
            {
                Name = "Treadmill",
                ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.MeterMinutes, 1320, 422),
            },
            new PersonalRecordMockup()
            {
                Name = "Leg Press",
                ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 160, 10),
            }
        ]);
    }
}