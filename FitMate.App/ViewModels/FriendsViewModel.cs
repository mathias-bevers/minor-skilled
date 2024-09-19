using System.Collections.ObjectModel;

namespace FitMate.ViewModels;

public class FriendsViewModel
{
    public ObservableCollection<UserMockup> Friends { get; set; }

    public FriendsViewModel()
    {
        Friends = new ObservableCollection<UserMockup>([
            new UserMockup() { Username = "John_Doe", Age = 21, Gender = "Male" },
            new UserMockup() { Username = "Jane_Doe", Age = 35, Gender = "Female" },
            new UserMockup() { Username = "Jesse_Doe", Age = 21, Gender = "Non-Binary" },
        ]);
    }
}