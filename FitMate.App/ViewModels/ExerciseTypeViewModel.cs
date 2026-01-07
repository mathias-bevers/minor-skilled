using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using FitMate.Utils;
using Microsoft.Data.SqlClient;

namespace FitMate.ViewModels;

public partial class ExerciseTypeViewModel : ObservableObject
{
    public ObservableCollection<string> MeasurementTypes { get; set; } = [];
    public ObservableCollection<string> MuscleTypes { get; set; } = [];

    private readonly Regex regex = new(@"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])");

    [ObservableProperty]
    private int selectedMeasurementType = -1;
    [ObservableProperty]
    private int selectedMuscleType = -1;
    [ObservableProperty]
    private string? exerciseName;

    public async Task LoadTypesFromDb()
    {
        MuscleTypes.Clear();
        bool hasRows = await SqlCommunicator.Select(new SqlCommand("SELECT Name FROM MuscleGroups"), reader =>
        {
            string name = Convert.ToString(reader["Name"]) ??
                          throw new SqlNullValueException("The value of [name] is null");
            MuscleTypes.Add(name);
        });

        if (!hasRows)
        {
            throw new PopupException("There are no MuscleGroups in the database!");
        }

        MeasurementTypes.Clear();
        hasRows = await SqlCommunicator.Select(new SqlCommand("SELECT Name FROM MeasurementTypes"), reader =>
        {
            string unsplit = Convert.ToString(reader["Name"]) ??
                             throw new SqlNullValueException("The value of [Name] is null");
            MeasurementTypes.Add(regex.Replace(unsplit, " "));
        });

        if (!hasRows)
        {
            throw new PopupException("There are no measurement types in the database!");
        }
    }

    public async Task<string> InsertExerciseType()
    {
        SqlCommand command = new("INSERT INTO ExerciseTypes (Name, MuscleGroupID, MeasurementTypeID) " +
                                 "VALUES (@name, @mgID, @mtID)");
        command.Parameters.AddWithValue("@name", ExerciseName);
        command.Parameters.AddWithValue("@mgID", SelectedMuscleType + 1);
        command.Parameters.AddWithValue("@mtID", SelectedMeasurementType + 1);

        await SqlCommunicator.Insert(command);
        ExerciseName = string.Empty;
        SelectedMeasurementType = SelectedMuscleType = -1;
        return $"Successfully created a new exercise preset with name '{ExerciseName}'";
    }
}