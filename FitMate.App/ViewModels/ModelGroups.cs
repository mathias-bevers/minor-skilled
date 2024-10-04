using FitMate.Models;

namespace FitMate.ViewModels;

public class ExerciseGroup(string name, List<Exercise> items) : ModelGroup<Exercise>(name, items);

public abstract class ModelGroup<T> : List<T>
{
    public string Name { get; }

    public ModelGroup(string name, List<T> items) : base(items)
    {
        Name = name;
    }
}