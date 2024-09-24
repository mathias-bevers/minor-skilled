using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace FitMate.ViewModels;

public class WorkoutsViewModel : ObservableObject
{
    public ObservableCollection<Mockups.WorkoutMockup> workouts { get; set; }

    public WorkoutsViewModel()
    {
        workouts = new ObservableCollection<Mockups.WorkoutMockup>([
            new Mockups.WorkoutMockup { Date = DateTime.Today, MusclesWorked = "Chest, Biceps" },
            new Mockups.WorkoutMockup { Date = DateTime.Today.AddDays(-2), MusclesWorked = "Legs" },
            new Mockups.WorkoutMockup { Date = DateTime.Today.AddDays(-5), MusclesWorked = "Shoulders, Triceps" }
        ]);
    }
}