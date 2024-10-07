using CommunityToolkit.Mvvm.ComponentModel;
using FitMate.Models;

namespace FitMate.ViewModels;

public class ExerciseGroup(string name, List<Exercise> items) : ModelGroup<Exercise>(name, items);

[ObservableObject]
public partial class ExerciseTypeGroup(string name, List<ExerciseType> items) : ModelGroup<ExerciseType>(name, items)
{
    public const string DOWN = "arrow_down_placeholder.png";
    public const string UP = "arrow_up_placeholder.png";
    
    [ObservableProperty]
    private string groupIcon = DOWN;
}

public abstract class ModelGroup<T> : List<T>
{
    public string Name { get; }

    public ModelGroup(string name, List<T> items) : base(items)
    {
        Name = name;
    }
}