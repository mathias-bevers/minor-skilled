using Encrypt.Library;
using FitMate.Models;
using FitMate.Utils;
using FitMate.ViewModels;

namespace FitMate.Views;

public partial class AllWorkoutsPage : ContentPage
{
    private AllWorkoutsViewModel ViewModel { get; } = new();

    public AllWorkoutsPage()
    {
        BindingContext = ViewModel;
        InitializeComponent();
        
        App.SetUserID();
        if (App.UserID == -1)
        {
            Shell.Current.GoToAsync("//Login");
        }
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
            if (AllWorkoutsViewModel.HasWorkoutForToday())
            {
                DisplayAlert("DOUBLE WORKOUT", "Cannot add a second workout for today!", "OK");
                return;
            }

            int workoutID = AllWorkoutsViewModel.InsertWorkout();

            ShellNavigationQueryParameters navigationParameters = new() { { "id", workoutID } };
            Shell.Current.GoToAsync("/Workout", navigationParameters);
        }
        catch (PopupException e)
        {
            DisplayAlert(e.Title, e.Message, "OK!");
        }
    }

    private void OnLogout(object? sender, EventArgs args)
    {
        App.DeleteUserID();
        Shell.Current.GoToAsync("//Login");
    }

    private void OnWorkoutSelected(object sender, SelectionChangedEventArgs args)
    {
        CollectionView cv = (CollectionView)sender;

        if (args.CurrentSelection.Count == 0 || ReferenceEquals(null, cv.SelectedItem))
        {
            return;
        }

        int workoutID = ((Workout)cv.SelectedItem).ID;

        ShellNavigationQueryParameters navigationParameters = new() { { "id", workoutID } };

        cv.SelectedItem = null;

        Shell.Current.GoToAsync("/Workout", navigationParameters);
    }
}