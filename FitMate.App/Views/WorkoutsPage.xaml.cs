namespace FitMate.Views;

public partial class WorkoutsPage : ContentPage
{
    private ViewModels.WorkoutsViewModel viewModel { get; }

    public WorkoutsPage()
    {
        InitializeComponent();

        viewModel = new ViewModels.WorkoutsViewModel();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    private void OnCreateNewWorkout(object? sender, EventArgs args)
    {
        Shell.Current.GoToAsync("/Workout"); //TODO: create a new workout entry.
    }

    private async void OnWorkoutSelected(object sender, SelectionChangedEventArgs args)
    {
        if (args.CurrentSelection.Count == 0) { return; }
        
        await Shell.Current.GoToAsync($"/Workout"); //TODO: goto correct workout
    }
}