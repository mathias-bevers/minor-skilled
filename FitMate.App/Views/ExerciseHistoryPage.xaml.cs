namespace FitMate.Views;

public partial class ExerciseHistoryPage : ContentPage
{
    ViewModels.ExerciseHistoryViewModel viewModel;
    
    public ExerciseHistoryPage()
    {
        InitializeComponent();
        Title = "History";
        
        viewModel = new ViewModels.ExerciseHistoryViewModel();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        viewModel.LoadHistoryFromDB();
        Title = $"{viewModel.PersonalRecord.ExerciseType.Name}'s History";
    }
}