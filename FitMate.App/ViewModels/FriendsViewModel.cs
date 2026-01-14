using System.Collections.ObjectModel;
using FitMate.Models;
using FitMate.Utils;
using Microsoft.Data.SqlClient;
using Nito.AsyncEx.Synchronous;

namespace FitMate.ViewModels;

public class FriendsViewModel
{
    public ObservableCollection<User> Following { get; set; } = [];

    public void SelectFollowsFromDB(int followerID)
    {
        Following.Clear();
        SqlCommand select = new("SELECT u.ID, u.UserName FROM Follow f INNER JOIN Users u ON f.FolloweeID = u.ID " +
                                "WHERE f.FollowerID = @follower_id");
        select.Parameters.AddWithValue("@follower_id", followerID);
        Task.Run(() => SqlCommunicator.Select(select, reader =>
        {
            User followee = new()
            {
                ID = Convert.ToInt32(reader["ID"]),
                UserName = Convert.ToString(reader["UserName"]) ?? "null"
            };
            Following.Add(followee);
        })).WaitAndUnwrapException();
    }
}