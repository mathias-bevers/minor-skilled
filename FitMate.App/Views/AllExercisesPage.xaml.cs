namespace FitMate.Views;

public partial class AllExercisesPage : ContentPage
{
    private ViewModels.AllExercisesViewModel ViewModel { get; } = new();

    public AllExercisesPage()
    {
        BindingContext = ViewModel;
        Title = "All Exercises";

        InitializeComponent();
    }
    
    private async void OnExerciseSelected(object? sender, SelectionChangedEventArgs args)
    {
        CollectionView cv = (CollectionView)sender;

        if (args.CurrentSelection.Count == 0 || cv.SelectedItem == null) { return; }

        Models.ExerciseType exerciseType = (Models.ExerciseType)cv.SelectedItem;

        if (string.IsNullOrEmpty(exerciseType.Name) || ViewModel.WorkoutID <= 0) { throw new InvalidDataException(); }

        ShellNavigationQueryParameters navigationParameters = new()
        {
            {"exercise_type_name", exerciseType.Name},
            {"exercise_type_id", exerciseType.ID},
            {"workout_id", ViewModel.WorkoutID},
        };

        cv.SelectedItem = null;

        await Shell.Current.GoToAsync("/Exercise", navigationParameters);
    }

    private void OnCreateNewExercisePreset(object sender, EventArgs args)
    {
        Shell.Current.GoToAsync("/ExerciseType");
    }
}