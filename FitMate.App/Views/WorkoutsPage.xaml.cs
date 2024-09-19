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

    private void OnCreateNewWorkout(object? sender, EventArgs e)
    {
        Shell.Current.GoToAsync("/Workout");
    }
}