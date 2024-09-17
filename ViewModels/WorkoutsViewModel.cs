using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace minor_skilled.ViewModels;

public struct WorkoutMockup
{
    public DateTime date { get; set; }
    public string musclesWorked { get; set; }
    
    public string dateString => date.ToString("dddd - dd/MM/yyyy");
}

public class WorkoutsViewModel : ObservableObject
{
    public ObservableCollection<WorkoutMockup> workouts { get; set; }

    public WorkoutsViewModel()
    {
        workouts = new ObservableCollection<WorkoutMockup>([
            new WorkoutMockup { date = DateTime.Today, musclesWorked = "Chest, Biceps" },
            new WorkoutMockup { date = DateTime.Today.AddDays(-2), musclesWorked = "Legs" },
            new WorkoutMockup { date = DateTime.Today.AddDays(-5), musclesWorked = "Shoulders, Triceps" }
        ]);
    }
}