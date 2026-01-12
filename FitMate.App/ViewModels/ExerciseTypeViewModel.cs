using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using FitMate.Utils;
using Microsoft.Data.SqlClient;
using Nito.AsyncEx.Synchronous;

namespace FitMate.ViewModels;

public partial class ExerciseTypeViewModel : ObservableObject
{
    public ObservableCollection<string> MeasurementTypes { get; set; } = [];
    public ObservableCollection<string> MuscleTypes { get; set; } = [];
    
    [ObservableProperty]
    private int selectedMeasurementType = -1;
    [ObservableProperty]
    private int selectedMuscleType = -1;
    [ObservableProperty]
    private string? exerciseName;

    public void SelectFromDB()
    {
        MuscleTypes.Clear();
        bool hasRows = Task.Run(() => SqlCommunicator.Select(new SqlCommand("SELECT Name FROM MuscleGroups"), reader =>
        {
            string name = Convert.ToString(reader["Name"]) ??
                          throw new SqlNullValueException("The value of [name] is null");
            MuscleTypes.Add(name);
        })).WaitAndUnwrapException();

        if (!hasRows)
        {
            throw new PopupException("There are no MuscleGroups in the database!");
        }

        MeasurementTypes.Clear();
        hasRows = Task.Run(() => SqlCommunicator.Select(new SqlCommand("SELECT Name FROM MeasurementTypes"), reader =>
        {
            string name = Convert.ToString(reader["Name"]) ??
                             throw new SqlNullValueException("The value of [Name] is null");
            MeasurementTypes.Add(name);
        })).WaitAndUnwrapException();

        if (!hasRows)
        {
            throw new PopupException("There are no measurement types in the database!");
        }
    }

    public string InsertExerciseType()
    {
        SqlCommand command = new("INSERT INTO ExerciseTypes (Name, MuscleGroupID, MeasurementTypeID) " +
                                 "VALUES (@name, @mgID, @mtID)");
        command.Parameters.AddWithValue("@name", ExerciseName);
        command.Parameters.AddWithValue("@mgID", SelectedMuscleType + 1);
        command.Parameters.AddWithValue("@mtID", SelectedMeasurementType + 1);

        Task.Run(() => SqlCommunicator.Insert(command)).WaitAndUnwrapException();
        string tmp = ExerciseName ?? "null";
        ExerciseName = string.Empty;
        SelectedMeasurementType = SelectedMuscleType = -1;
        return $"Successfully created a new exercise preset with name \'{tmp}\'";
    }
}