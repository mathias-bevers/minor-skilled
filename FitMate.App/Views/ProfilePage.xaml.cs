namespace FitMate.Views;

public partial class ProfilePage : ContentPage
{
    public ViewModels.ProfileViewModel viewModel { get; }

    public ProfilePage()
    {
        InitializeComponent();

        viewModel = new ViewModels.ProfileViewModel();
        BindingContext = viewModel;

        Title = ReferenceEquals(null, viewModel.User) ? "Not logged in..." : viewModel.User.UserName;
    }
}