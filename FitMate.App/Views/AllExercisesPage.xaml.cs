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

    private void OnExerciseSelected(object? sender, SelectionChangedEventArgs args)
    {
        if (sender is not CollectionView cv)
        {
            throw new NullReferenceException("collection view is null!");
        }

        if (args.CurrentSelection.Count == 0 || cv.SelectedItem == null)
        {
            return;
        }

        Models.ExerciseType exerciseType = (Models.ExerciseType)cv.SelectedItem;

        if (exerciseType.ID <= 0 || ViewModel.WorkoutID <= 0)
        {
            throw new InvalidDataException();
        }

        ShellNavigationQueryParameters navigationParameters = new()
        {
            { "exercise_type_name", exerciseType.Name },
            { "exercise_type_id", exerciseType.ID },
            { "workout_id", ViewModel.WorkoutID }
        };

        cv.SelectedItem = null;

        Shell.Current.GoToAsync("/Exercise", navigationParameters);
    }
    
    private void OnCreateNewExercisePreset(object sender, EventArgs args)
    {
        Shell.Current.GoToAsync("/ExerciseType");
    }

    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        SearchBar searchBar = sender as SearchBar ?? throw new NullReferenceException();
        list.ItemsSource = ViewModel.GetSearchResults(searchBar.Text);
    }
}