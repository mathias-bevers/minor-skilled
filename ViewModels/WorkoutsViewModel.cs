using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace minor_skilled.ViewModels;

public class WorkoutsViewModel : ObservableObject
{
    public ObservableCollection<WorkoutMockup> workouts { get; set; }

    public WorkoutsViewModel()
    {
        workouts = new ObservableCollection<WorkoutMockup>([
            new WorkoutMockup { Date = DateTime.Today, MusclesWorked = "Chest, Biceps" },
            new WorkoutMockup { Date = DateTime.Today.AddDays(-2), MusclesWorked = "Legs" },
            new WorkoutMockup { Date = DateTime.Today.AddDays(-5), MusclesWorked = "Shoulders, Triceps" }
        ]);
    }
}