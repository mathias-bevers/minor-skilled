namespace FitMate.Views;

public partial class ProfilePage : ContentPage
{
    public ViewModels.ProfileViewModel viewModel { get; }

    public ProfilePage()
    {
        InitializeComponent();

        viewModel = new ViewModels.ProfileViewModel();
        BindingContext = viewModel;

        Title = "Profile";
    }

    protected override void OnAppearing()
    {
        viewModel.LoadFromDbAsync();
    }
}