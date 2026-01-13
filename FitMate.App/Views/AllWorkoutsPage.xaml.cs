using CommunityToolkit.Maui.Views;
using FitMate.Models;
using FitMate.Utils;
using FuzzySharp;

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
        base.OnAppearing();
        ViewModel.OnAppearing();
    }

    private void OnCreateNewWorkout(object? sender, EventArgs args)
    {
        try
        {
            int workoutID = Task.Run(ViewModel.InsertWorkoutAsync).Result;
            ShellNavigationQueryParameters navigationParameters = new() { { "id", workoutID } };
            Shell.Current.GoToAsync("/Workout", navigationParameters);
        }
        catch (PopupException e)
        {
            DisplayAlert(e.Title, e.Message, "OK!");
        }
    }

    private async void OnWorkoutSelected(object sender, SelectionChangedEventArgs args)
    {
        CollectionView cv = (CollectionView)sender;

        if (args.CurrentSelection.Count == 0 || ReferenceEquals(null, cv.SelectedItem))
        {
            return;
        }

        int workoutID = ((Workout)cv.SelectedItem).ID;

        ShellNavigationQueryParameters navigationParameters = new() { { "id", workoutID } };

        cv.SelectedItem = null;

        await Shell.Current.GoToAsync("/Workout", navigationParameters);
    }
}