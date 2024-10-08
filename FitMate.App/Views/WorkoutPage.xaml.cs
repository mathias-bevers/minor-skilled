
namespace FitMate.Views;

public partial class WorkoutPage : ContentPage
{
    private readonly ViewModels.WorkoutModelView viewModel;

    public WorkoutPage()
    {
        InitializeComponent();
        Title = "Workout";

        viewModel = new ViewModels.WorkoutModelView();
        BindingContext = viewModel;
    }

    private async void OnAddExerciseClicked(object? sender, EventArgs args)
    {
        await Shell.Current.GoToAsync("/AllExercises");
    }

    private async void OnExerciseSelected(object sender, SelectionChangedEventArgs args)
    {
        CollectionView cv = (CollectionView)sender;

        if (args.CurrentSelection.Count == 0 || cv.SelectedItem == null) { return; }

        Models.Exercise exercise = (Models.Exercise)cv.SelectedItem;

        ShellNavigationQueryParameters navigationParameters = new()
        {
            {"exercise_type_id", exercise.ExerciseType.ID},
            {"workout_id", viewModel.WorkoutID},
        };

        cv.SelectedItem = null;

        await Shell.Current.GoToAsync("/Exercise", navigationParameters);
    }
}