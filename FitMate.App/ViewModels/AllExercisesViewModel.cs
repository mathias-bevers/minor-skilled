using System.Collections.ObjectModel;
using Android.Content;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitMate.ViewModels.Mockups;

namespace FitMate.ViewModels;

public partial class AllExercisesViewModel : ObservableObject
{
    public ObservableCollection<ExerciseGroupMockup> Exercises { get; set; }
    private readonly List<ExerciseMockup> unsortedExercises;


    public AllExercisesViewModel()
    {
        unsortedExercises =
        [
            new ExerciseMockup { Name = "Treadmill", MuscleGroup = MuscleGroup.Cardio },
            new ExerciseMockup { Name = "Hammer Curl", MuscleGroup = MuscleGroup.Biceps },
            new ExerciseMockup { Name = "Close Grip Curl", MuscleGroup = MuscleGroup.Biceps },
            new ExerciseMockup { Name = "Preacher Curl", MuscleGroup = MuscleGroup.Biceps },
            new ExerciseMockup { Name = "Incline Bench Press", MuscleGroup = MuscleGroup.Chest },
            new ExerciseMockup { Name = "Decline Bench Press", MuscleGroup = MuscleGroup.Chest },
            new ExerciseMockup { Name = "Cable Fly", MuscleGroup = MuscleGroup.Chest }
        ];

        List<ExerciseGroupMockup> exercises = [];

        for (int i = 0; i < unsortedExercises.Count; ++i)
        {
            ExerciseMockup exercise = unsortedExercises[i];
            bool isAdded = false;

            for (int ii = 0; ii < exercises.Count; ++ii)
            {
                ExerciseGroupMockup group = exercises[ii];
                if (exercise.MuscleGroup.ToString() != group.Name) { continue; }

                isAdded = true;
                group.Add(exercise);
                break;
            }

            if (isAdded) { continue; }

            exercises.Add(new ExerciseGroupMockup(exercise.MuscleGroup.ToString(), [exercise]));
        }
        
        Exercises = new ObservableCollection<ExerciseGroupMockup>(exercises);
    }

    [RelayCommand]
    private void ToggleGroupData(ExerciseGroupMockup group)
    {
        if (group.GroupIcon == "arrow_down_placeholder.png")
        {
            group.Clear();
            group.GroupIcon = "arrow_up_placeholder.png";
        }
        else
        {
            List<ExerciseMockup> toAdd = new();
            for (int i = 0; i < unsortedExercises.Count; ++i)
            {
                ExerciseMockup exercise = unsortedExercises[i];
                if(exercise.MuscleGroup.ToString() != group.Name) { continue; }
                toAdd.Add(exercise);
            }
            
            group.AddRange(toAdd);
            group.GroupIcon = "arrow_down_placeholder.png";
        }
    }
}