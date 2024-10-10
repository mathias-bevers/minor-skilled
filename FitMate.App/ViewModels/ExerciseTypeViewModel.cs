using CommunityToolkit.Mvvm.ComponentModel;

namespace FitMate.ViewModels;

public class ExerciseTypeViewModel : ObservableObject
{
    public List<string> MuscleTypes { get; set; }
    public int SelectedMuscleType { get; set; } = -1;
    public List<string> MeasurementTypes { get; set; }
    public int SelectedMeasurementType { get; set; } = -1;
    
    public string? ExerciseName { get; set; }

    public ExerciseTypeViewModel()
    {
        // MuscleTypes = [..Enum.GetNames<Mockups.MuscleGroup>()]; //TODO: load names from db.
        MeasurementTypes = [..Enum.GetNames<Mockups.ExerciseSet.SetType>()];
    }
}