using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace FitMate.ViewModels;

public partial class ExerciseViewModel : ObservableObject
{
    public ObservableCollection<Mockups.ExerciseMockup> Exercises { get; set; }

    [ObservableProperty] private string? kgsOrMtr;
    [ObservableProperty] private string? repsOrSecs;

    public ExerciseViewModel()
    {
        Exercises = new ObservableCollection<Mockups.ExerciseMockup>([
            new Mockups.ExerciseMockup
            {
                Name = "Hammer Curl",
                ExerciseSet = new Mockups.ExerciseSet((int)Mockups.ExerciseSet.SetType.KiloReps, 10, 12),
                IsPersonalRecord = false
            },
            new Mockups.ExerciseMockup
            {
                Name = "Hammer Curl",
                ExerciseSet = new Mockups.ExerciseSet((int)Mockups.ExerciseSet.SetType.KiloReps, 10, 12),
                IsPersonalRecord = false
            },
            new Mockups.ExerciseMockup
            {
                Name = "Hammer Curl",
                ExerciseSet = new Mockups.ExerciseSet((int)Mockups.ExerciseSet.SetType.KiloReps, 12, 12),
                IsPersonalRecord = true
            }
        ]);
    }
}