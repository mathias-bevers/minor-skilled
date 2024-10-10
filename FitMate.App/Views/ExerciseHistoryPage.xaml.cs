namespace FitMate.Views;

public partial class ExerciseHistoryPage : ContentPage
{
    private ViewModels.ExerciseHistoryViewModel ViewModel { get; } = new();
    
    public ExerciseHistoryPage()
    {
        BindingContext = ViewModel;
        Title = "History";
        ViewModel.UpdateTitleEvent += title => Title = title;
        
        InitializeComponent();
    }
}