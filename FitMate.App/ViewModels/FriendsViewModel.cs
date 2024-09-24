using System.Collections.ObjectModel;

namespace FitMate.ViewModels;

public class FriendsViewModel
{
    public ObservableCollection<Mockups.UserMockup> Friends { get; set; }

    public FriendsViewModel()
    {
        Friends = new ObservableCollection<Mockups.UserMockup>([
            new Mockups.UserMockup() { Username = "John_Doe", Age = 21, Gender = "Male" },
            new Mockups.UserMockup() { Username = "Jane_Doe", Age = 35, Gender = "Female" },
            new Mockups.UserMockup() { Username = "Jesse_Doe", Age = 21, Gender = "Non-Binary" },
        ]);
    }
}