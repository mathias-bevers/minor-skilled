namespace FitMate.Views;

public partial class WorkoutPage : ContentPage
{
    private ViewModels.WorkoutModelView ViewModel { get; } = new();

    public WorkoutPage()
    {
        Title = "Workout";
        BindingContext = ViewModel;

        InitializeComponent();
    }

    private void OnAddExerciseClicked(object? sender, EventArgs args)
    {
        ShellNavigationQueryParameters navigationQueryParameters = new()
        {
            { "workout_id", ViewModel.WorkoutID }
        };

        Task.Run(() => Shell.Current.GoToAsync("/AllExercises", navigationQueryParameters));
    }

    private void OnExerciseSelected(object sender, SelectionChangedEventArgs args)
    {
        CollectionView cv = (CollectionView)sender;

        if (args.CurrentSelection.Count == 0 || cv.SelectedItem == null)
        {
            return;
        }

        Models.Exercise exercise = (Models.Exercise)cv.SelectedItem;

        ShellNavigationQueryParameters navigationParameters = new()
        {
            { "exercise_type_name", exercise.ExerciseType.Name },
            { "exercise_type_id", exercise.ExerciseType.ID },
            { "workout_id", ViewModel.WorkoutID }
        };

        cv.SelectedItem = null;

        Task.Run(() => Shell.Current.GoToAsync("/Exercise", navigationParameters));
    }
}