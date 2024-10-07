using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitMate.Models;
using Microsoft.Data.SqlClient;

namespace FitMate.ViewModels;

public partial class AllExercisesViewModel : ObservableObject
{
    public ObservableCollection<ExerciseTypeGroup> ExerciseTypes { get; set; } = [];
    private readonly List<ExerciseType> unsortedTypes = [];

    public void LoadExercisesFromDB()
    {
        unsortedTypes.Clear();
        using (SqlConnection connection = new(App.SERVER_SETTINGS.ConnectionString))
        {
            connection.Open();

            using (SqlCommand command = new(GenerateAllExercisesQuery(), connection))
            {
                SqlDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    connection.Close();
                    return;
                }

                while (reader.Read())
                {
                    ExerciseType type = new()
                    {
                        Name = (string)reader["typeName"],
                        MuscleGroup = new Models.MuscleGroup
                        {
                            Name = (string)reader["muscleGroupName"]
                        }
                    };

                    unsortedTypes.Add(type);
                }
            }

            connection.Close();
        }

        foreach (ExerciseTypeGroup group in unsortedTypes.GroupBy(t => t.MuscleGroup.Name)
                     .Select(g => new ExerciseTypeGroup(g.Key, g.ToList()))) { ExerciseTypes.Add(group); }
    }

    private string GenerateAllExercisesQuery() =>
        "SELECT et.Name as typeName, et.MuscleGroupID, mg.Name as muscleGroupName FROM ExercisesTypes et " +
        "JOIN MuscleGroups mg ON et.MuscleGroupID = mg.ID;";

    [RelayCommand]
    private void ToggleGroupData(ExerciseTypeGroup group)
    {
        if (group.GroupIcon == ExerciseTypeGroup.DOWN)
        {
            group.Clear();
            group.GroupIcon = ExerciseTypeGroup.UP;
        }
        else
        {
            List<ExerciseType> toAdd = new();
            for (int i = 0; i < unsortedTypes.Count; ++i)
            {
                ExerciseType exercise = unsortedTypes[i];
                if (exercise.MuscleGroup.Name != group.Name) { continue; }

                toAdd.Add(exercise);
            }

            group.AddRange(toAdd);
            group.GroupIcon = ExerciseTypeGroup.DOWN;
        }
    }
}