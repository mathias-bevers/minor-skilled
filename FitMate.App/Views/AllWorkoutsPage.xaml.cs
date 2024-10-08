using FitMate.Models;

namespace FitMate.Views;

public partial class AllWorkoutsPage : ContentPage
{
    private ViewModels.AllWorkoutsViewModel viewModel { get; }

    public AllWorkoutsPage()
    {
        InitializeComponent();

        viewModel = new ViewModels.AllWorkoutsViewModel();
        BindingContext = viewModel;
    }

    private void OnCreateNewWorkout(object? sender, EventArgs args)
    {
        Shell.Current.GoToAsync("/Workout"); //TODO: create a new workout entry.
    }

    private async void OnWorkoutSelected(object sender, SelectionChangedEventArgs args)
    {
        CollectionView cv = (CollectionView)sender;

        if (args.CurrentSelection.Count == 0 || ReferenceEquals(null, cv.SelectedItem)) { return; }

        int workoutID = ((Workout)cv.SelectedItem).ID;

        ShellNavigationQueryParameters navigationParameters = new() { { "id", workoutID } };

        cv.SelectedItem = null;

        await Shell.Current.GoToAsync($"/Workout", navigationParameters);
    }
}