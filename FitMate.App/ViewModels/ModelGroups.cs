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
    private string groupIcon = UP;
}

public abstract class ModelGroup<T> : List<T>
{
    public string Name { get; }

    public ModelGroup(string name, List<T> items) : base(items)
    {
        Name = name;
    }
}

public static class ModelGroupExtensions
{
    public static T Find<M, T>(this IList<M> group, Func<T, bool> comparer) where M : ModelGroup<T>
    {
        for (int i = 0; i < group.Count; ++i)
        {
            for (int ii = 0; ii < group[i].Count; ++ii)
            {
                T element = group[i][ii];
                if (!comparer(element))
                {
                    continue;
                }

                return element;
            }
        }

        throw new Exception("Could not find expected element in group");
    }
}