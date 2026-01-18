using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FitMate.Models;
using FitMate.Utils;
using Microsoft.Data.SqlClient;
using Nito.AsyncEx.Synchronous;

namespace FitMate.ViewModels;

public partial class ProfileViewModel : ObservableObject, IQueryAttributable
{
    public ObservableCollection<Exercise> PersonalRecords { get; set; } = [];

    private bool fromQuery = false;
    private bool hasBeenInitialized = false; 
    [ObservableProperty]
    private bool isOwnProfile = false;

    [ObservableProperty]
    private bool isSharingPR = false;
    
    private int userID = -1;
    [ObservableProperty]
    private User user = new();

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        userID = -1;
        if (query.TryGetValue("user_id", out object? value))
        {
            userID = Convert.ToInt32(value);
        }

        SelectUser(userID);
        SelectPersonalRecords(User.SharePR);
        fromQuery = true;
    }

    public void OnAppearing()
    {
        if (fromQuery)
        {
            fromQuery = false;
            return;
        }

        userID = App.UserID;

        SelectUser(userID);
        SelectPersonalRecords(true);
    }

    private void SelectPersonalRecords(bool showPRs)
    {
        PersonalRecords.Clear();
        IsSharingPR = showPRs || IsOwnProfile;

        if (!IsSharingPR)
        {
            return;
        }

        Exercise[] prs = PersonalRecordFinder.FindAll(userID);
        for (int i = 0; i < prs.Length; ++i)
        {
            PersonalRecords.Add(prs[i]);
        }
    }

    private void SelectUser(int userID)
    {
        SqlCommand select = new("SELECT u.UserName, u.GenderID, u.DateOfBirth, u.SharePR " +
                                "FROM Users u WHERE ID = @user_id");
        select.Parameters.AddWithValue("@user_id", userID);

        Task.Run(() => SqlCommunicator.Select(select, reader =>
        {
            User = new User
            {
                UserName = Convert.ToString(reader["UserName"]) ?? "null",
                GenderID = Convert.ToInt32(reader["GenderID"]),
                DateOfBirth = Convert.ToString(reader["DateOfBirth"]) ?? "null",
                SharePR = Convert.ToBoolean(reader["SharePR"])
            };
        })).WaitAndUnwrapException();

        IsOwnProfile = userID == App.UserID;
    }

    public void UpdateShowPR(bool value)
    {
        if (!hasBeenInitialized)
        {
            hasBeenInitialized = true;
            return;
        }
        
        SqlCommand update = new("UPDATE Users SET SharePR = @share_pr WHERE ID = @user_id");
        update.Parameters.AddWithValue("@share_pr", Convert.ToInt32(value));
        update.Parameters.AddWithValue("@user_id", userID);

        Task.Run(() => SqlCommunicator.Update(update)).WaitAndUnwrapException();
        SelectUser(userID);
    }
}