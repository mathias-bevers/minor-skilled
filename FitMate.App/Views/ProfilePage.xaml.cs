namespace FitMate.Views;

public partial class ProfilePage : ContentPage
{
    public ViewModels.ProfileViewModel ViewModel { get; } = new();

    public ProfilePage()
    {
        Title = "Profile";
        BindingContext = ViewModel;
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        ViewModel.SelectUser();
        ViewModel.SelectPersonalRecords();
        base.OnAppearing();
    }
}