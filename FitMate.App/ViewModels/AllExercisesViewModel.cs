using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using FitMate.Models;
using FitMate.Utils;
using FuzzySharp;
using Microsoft.Data.SqlClient;
using Nito.AsyncEx.Synchronous;

namespace FitMate.ViewModels;

public class AllExercisesViewModel : ObservableObject, IQueryAttributable
{
    public int WorkoutID { get; private set; }
    public ObservableCollection<ExerciseTypeGroup> ExerciseTypes { get; set; } = [];
    private readonly List<ExerciseType> exerciseTypes = [];

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("workout_id", out object? value))
        {
            WorkoutID = Convert.ToInt32(value);
        }

        SelectFromDB();
    }

    private void SelectFromDB()
    {
        SqlCommand command = new("SELECT * FROM MuscleGroups");
        Task.Run(() => SqlCommunicator.Select(command, reader =>
        {
            ExerciseTypeGroup group = new(Convert.ToString(reader["Name"]) ?? "null", []);
            ExerciseTypes.Add(group);
        })).WaitAndUnwrapException();

        command = new SqlCommand("SELECT et.ID, et.Name AS type_name, mg.Name AS group_name, mg.ID as group_id" +
                                 " FROM ExerciseTypes et INNER JOIN MuscleGroups mg ON et.MuscleGroupID = mg.ID");
        Task.Run(() => SqlCommunicator.Select(command, reader =>
        {
            ExerciseType type = new()
            {
                ID = Convert.ToInt32(reader["ID"]),
                Name = Convert.ToString(reader["type_name"]) ?? "null",
                MuscleGroup = new MuscleGroup
                {
                    ID = Convert.ToInt32(reader["group_id"]),
                    Name = Convert.ToString(reader["group_name"]) ?? "null"
                }
            };

            exerciseTypes.Add(type);
        })).WaitAndUnwrapException();

        GroupExerciseTypes(exerciseTypes);
    }

    public List<ExerciseTypeGroup> GetSearchResults(string searchText)
    {
        if (string.IsNullOrEmpty(searchText))
        {
            GroupExerciseTypes(exerciseTypes);
            return ExerciseTypes.ToList();
        }

        List<ExerciseType> searchResult = [..exerciseTypes];
        searchText = searchText.ToLower();
        
        for (int i = searchResult.Count - 1; i >= 0; --i)
        {
            string name = searchResult[i].Name.ToLower();
            if (name.Contains(searchText))
            {
                continue;
            }

            int fuzzyScore = Fuzz.Ratio(searchText, name);
            
            if (fuzzyScore >= 50)
            {
                continue;    
            }
            
            searchResult.RemoveAt(i);
        }
        
        GroupExerciseTypes(searchResult);
        return ExerciseTypes.ToList();
    }

    private void GroupExerciseTypes(List<ExerciseType> exerciseTypes)
    {
        for (int i = 0; i < ExerciseTypes.Count; ++i)
        {
            ExerciseTypes[i].Clear();
        }

        for (int i = 0; i < exerciseTypes.Count; ++i)
        {
            ExerciseType exerciseType = exerciseTypes[i];
            ExerciseTypes[exerciseType.MuscleGroup.ID - 1].Add(exerciseType);
        }
    }
}