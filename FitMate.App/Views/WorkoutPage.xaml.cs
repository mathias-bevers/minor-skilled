namespace FitMate.Views;

public partial class WorkoutPage : ContentPage
{
    private ViewModels.WorkoutModelView viewModel;
    
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

    private void OnExerciseSelected(object? sender, SelectionChangedEventArgs e)
    {
        Shell.Current.GoToAsync("/Exercise");
    }
}