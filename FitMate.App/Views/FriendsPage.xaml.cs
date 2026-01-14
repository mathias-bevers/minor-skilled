using System.Text.RegularExpressions;

namespace FitMate.Views;

public partial class FriendsPage : ContentPage
{
    public const string USERNAME_REGEX = "^[a-zA-Z0-9_.-]+$";

    private readonly ViewModels.FriendsViewModel viewModel = new();

    public FriendsPage()
    {
        InitializeComponent();

        Title = "Friends";
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.SelectFollowsFromDB(App.USER_ID);
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

    private void OnFolloweeSelected(object sender, SelectionChangedEventArgs args)
    {
        CollectionView cv = (CollectionView)sender;

        if (args.CurrentSelection.Count == 0 || ReferenceEquals(null, cv.SelectedItem))
        {
            return;
        }

        int userID = ((Models.User)cv.SelectedItem).ID;
        ShellNavigationQueryParameters navigationParameters = new() { { "user_id", userID } };

        Shell.Current.GoToAsync("/Profile", navigationParameters);
    }
}