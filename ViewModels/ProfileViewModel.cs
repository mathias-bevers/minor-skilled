using System.Collections.ObjectModel;

namespace minor_skilled.ViewModels;

public class ProfileViewModel
{
    public ProfileMockup UserPlaceholder { get; set; }
    public ObservableCollection<PersonalRecordMockup> PlaceholderPRs { get; set; }

    public ProfileViewModel()
    {
        UserPlaceholder = new ProfileMockup { Username = "John_Doe", Age = 21, Gender = "Male" };

        PlaceholderPRs = new ObservableCollection<PersonalRecordMockup>([
            new PersonalRecordMockup()
            {
                Name = "Bicep Curl",
                Measurement = 14,
                Value = 12,
                Type = 0
            },
            new PersonalRecordMockup()
            {
                Name = "Treadmill",
                Measurement = 1320,
                Value = 420,
                Type = 0
            },
            new PersonalRecordMockup()
            {
                Name = "Leg Press",
                Measurement = 160,
                Value = 10,
                Type = 0
            }
        ]);
    }
}

public struct ProfileMockup
{
    public string Username { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }
}

public struct PersonalRecordMockup
{
    public string Name { get; set; }
    public int Measurement { get; set; }
    public int Value { get; set; }
    public int Type { get; set; }
}