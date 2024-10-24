using FitMate.Models;

namespace FitMate.Views;

public partial class AllWorkoutsPage : ContentPage
{
    private ViewModels.AllWorkoutsViewModel ViewModel { get; } = new();

    public AllWorkoutsPage()
    {
        BindingContext = ViewModel;
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        ViewModel.OnAppearing();
        base.OnAppearing();
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