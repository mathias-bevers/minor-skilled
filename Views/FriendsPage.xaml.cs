using System.Diagnostics;
using System.Text.RegularExpressions;

namespace minor_skilled.Views;

public partial class FriendsPage : ContentPage
{
    public const string USERNAME_REGEX = "^[a-zA-Z0-9_.-]+$";
    
    private ViewModels.FriendsViewModel viewModel { get; }

    public FriendsPage()
    {
        InitializeComponent();

        Title = "Friends";

        viewModel = new ViewModels.FriendsViewModel();
        BindingContext = viewModel;
    }

    private void OnFriendSearch(object sender, EventArgs e)
    {
        Entry entry = (Entry)sender;
        string input = entry.Text;
        
        if (string.IsNullOrEmpty(input))
        {
            DisplayAlert("Invalid Input", "Please enter a name", "OK");
            return;
        }

        if (!Regex.IsMatch(input, USERNAME_REGEX))
        {
            DisplayAlert("Invalid Input", "A username can only contain letters numbers and underscores(_)", "OK");
            return;
        }

        if (!string.Equals(input, "mathias")) //TODO: database search.
        {
            DisplayAlert("Unknown User", $"There is no user with username \'{input}\'", "OK");
            return;
        }
        
        DisplayAlert("Friend Request Sent", $"A friend request was sent to \'{input}\'", "OK");
        entry.Text = string.Empty;
    }
}