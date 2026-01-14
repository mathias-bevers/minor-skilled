using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FitMate.Models;
using FitMate.Utils;
using Microsoft.Data.SqlClient;
using Nito.AsyncEx.Synchronous;

namespace FitMate.ViewModels;

public partial class ProfileViewModel : ObservableObject, IQueryAttributable
{
    public bool HasBeenSet { get; set; } = false;
    public ObservableCollection<Exercise> PersonalRecords { get; set; } = [];
    
    [ObservableProperty]
    private bool isSharingPR = false;
    private int userID = App.USER_ID;
    [ObservableProperty]
    private User user = new();

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("user_id", out object? value))
        {
            userID = Convert.ToInt32(value);
        }

        SelectUser();
        SelectPersonalRecords(User.SharePR);
        HasBeenSet = true;
    }

    public void SelectPersonalRecords(bool showPRs)
    {
        PersonalRecords.Clear();
        IsSharingPR = showPRs;

        if (!showPRs)
        {
            return;
        }

        Exercise[] prs = PersonalRecordFinder.FindAll(userID);
        for (int i = 0; i < prs.Length; ++i)
        {
            PersonalRecords.Add(prs[i]);
        }
    }

    public void SelectUser()
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
    }
}