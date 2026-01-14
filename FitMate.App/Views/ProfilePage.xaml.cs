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
        if (ViewModel.HasBeenSet)
        {
            ViewModel.HasBeenSet = false;
        }
        else
        {
            ViewModel.SelectUser();
            ViewModel.SelectPersonalRecords(true);
        }

        base.OnAppearing();
    }
}