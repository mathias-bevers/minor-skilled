using FitMate.Utils;

namespace FitMate.Views;

public partial class LoginRegisterPage : ContentPage
{
    private ViewModels.LoginViewModel viewModel = new();
    
    public LoginRegisterPage()
    {
        InitializeComponent();
        
        Title = "Login / Register";
        BindingContext = viewModel;
    }

    private void OnLoginClicked(object? sender, EventArgs e)
    {
        try
        {
            viewModel.OnLoginRequest();
        }
        catch (PopupException exception)
        {
            DisplayAlert(exception.Title, exception.Message, "OK");
        }
    }
    
    private void OnRegisterClicked(object? sender, EventArgs e)
    {
        viewModel.OnRegisterRequest();
    }
}