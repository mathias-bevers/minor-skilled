using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FitMate.Models;

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
                ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 10, 12),
                IsPersonalRecord = false
            },
            new Mockups.ExerciseMockup
            {
                Name = "Hammer Curl",
                ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 10, 12),
                IsPersonalRecord = false
            },
            new Mockups.ExerciseMockup
            {
                Name = "Hammer Curl",
                ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 12, 12),
                IsPersonalRecord = true
            }
        ]);
    }
}