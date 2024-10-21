using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitMate.Models;
using Microsoft.Data.SqlClient;

namespace FitMate.ViewModels;

public partial class AllExercisesViewModel : ObservableObject, IQueryAttributable
{
    public int WorkoutID { get; private set; }
    public ObservableCollection<ExerciseTypeGroup> ExerciseTypes { get; set; } = [];
    private readonly List<ExerciseType> unsortedTypes = [];

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("workout_id", out object? value)) { WorkoutID = Convert.ToInt32(value); }

        Task.Run(LoadExercisesFromDB);
    }

    private async Task LoadExercisesFromDB()
    {
        unsortedTypes.Clear();
        ExerciseTypes.Clear();
        await using (SqlConnection connection = new(App.SETTINGS.Server.ConnectionString))
        {
            connection.Open();

            await using (SqlCommand command = new(GenerateAllExercisesQuery(), connection))
            {
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (!reader.HasRows)
                {
                    connection.Close();
                    return;
                }

                while (reader.Read())
                {
                    ExerciseType type = new()
                    {
                        Name = Convert.ToString(reader["typeName"]) ??
                               throw new SqlNullValueException("reader[\"typeName\"]"),
                        MuscleGroup = new MuscleGroup
                        {
                            Name = Convert.ToString(reader["muscleGroupName"]) ??
                                   throw new SqlNullValueException("reader[\"muscleGroupName\"]")
                        }
                    };

                    unsortedTypes.Add(type);
                }
            }

            connection.Close();
        }

        foreach (ExerciseTypeGroup group in unsortedTypes.GroupBy(t => t.MuscleGroup.Name)
                     .Select(g => new ExerciseTypeGroup(g.Key, []))) { ExerciseTypes.Add(group); }
    }

    private string GenerateAllExercisesQuery() =>
        "SELECT et.Name as typeName, mg.Name as muscleGroupName FROM ExerciseTypes et " +
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
            List<ExerciseType> toAdd = [];
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