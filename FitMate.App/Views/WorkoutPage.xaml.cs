namespace FitMate.Views;

public partial class WorkoutPage : ContentPage
{
    private ViewModels.WorkoutModelView viewModel
        ;
    
    public WorkoutPage()
    {
        InitializeComponent();
        Title = "Workout";
        
        viewModel = new ViewModels.WorkoutModelView();
        BindingContext = viewModel;
    }
}