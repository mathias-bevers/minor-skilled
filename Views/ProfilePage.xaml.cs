namespace minor_skilled.Views;

public partial class ProfilePage : ContentPage
{
    public ViewModels.ProfileViewModel viewModel { get; }

    public ProfilePage()
    {
        InitializeComponent();

        Title = "Profile"; //TODO: load name of current loaded user.

        viewModel = new ViewModels.ProfileViewModel();
        BindingContext = viewModel;
    }
}