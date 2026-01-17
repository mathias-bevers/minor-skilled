using FitMate.Utils;

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
        base.OnAppearing();
        ViewModel.OnAppearing();
    }

    private void OnShowPRToggle(object? sender, CheckedChangedEventArgs e)
    {
        try
        {
            ViewModel.UpdateShowPR(e.Value);
        }
        catch (PopupException exception)
        {
            DisplayAlert(exception.Title, exception.Message, "OK");
        }
    }
}