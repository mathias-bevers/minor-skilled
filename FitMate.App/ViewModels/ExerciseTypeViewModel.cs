using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using FitMate.Utils;
using Microsoft.Data.SqlClient;

namespace FitMate.ViewModels;

public partial class ExerciseTypeViewModel : ObservableObject
{
    public int SelectedMeasurementType { get; set; } = -1;
    public int SelectedMuscleType { get; set; } = -1;
    public ObservableCollection<string> MeasurementTypes { get; set; } = [];
    public ObservableCollection<string> MuscleTypes { get; set; } = [];

    [ObservableProperty]
    private string? exerciseName;

    public async Task LoadTypesFromDb()
    {
        await using SqlConnection connection = new(App.SETTINGS.Server.ConnectionString);
        await using SqlCommand command = new();
        command.Connection = connection;
        SqlDataReader reader;

        try
        {
            connection.Open();
            
            MuscleTypes.Clear();
            command.CommandText = "SELECT Name FROM MuscleGroups";
            reader = await command.ExecuteReaderAsync();

            if (!reader.HasRows)
            {
                reader.Close();
                throw new PopupException("There are no MuscleGroups in the database!");
            }

            while (reader.Read())
            {
                string name = Convert.ToString(reader["Name"]) ??
                              throw new SqlNullValueException("The value of [name] is null");
                MuscleTypes.Add(name);
            }

            reader.Close();
        }
        catch (Exception exception)
        {
            connection.Close();

            if (exception is PopupException) { throw; }

            System.Diagnostics.Debug.WriteLine(exception.Message);
            return;
        }

        try
        {
            MeasurementTypes.Clear();
            command.CommandText = "SELECT Name FROM MeasurementTypes";
            reader = await command.ExecuteReaderAsync();
            Regex regex = new(@"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])");

            if (!reader.HasRows)
            {
                reader.Close();
                throw new PopupException("There are no MeasurmentTypes in the database!");
            }

            while (reader.Read())
            {
                string unsplitName = Convert.ToString(reader["Name"]) ??
                                     throw new SqlNullValueException("The value of [Name] is null");
                MeasurementTypes.Add(regex.Replace(unsplitName, " "));
            }
        }
        catch (Exception exception)
        {
            connection.Close();

            if (exception is PopupException) { throw; }

            System.Diagnostics.Debug.WriteLine(exception.Message);
        }

        connection.Close();
    }
}