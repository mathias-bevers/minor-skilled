namespace FitMate.Models;

public class Follow
{
    public int FollowerID { get; set; }
    public User Follower { get; set; } = null!;
    
    public int FolloweeID { get; set; }
    public User Followee { get; set; } = null!;
}