using System.Collections.ObjectModel;

namespace minor_skilled.ViewModels;

public class ProfileViewModel
{
    public UserMockup UserPlaceholder { get; set; }
    public ObservableCollection<PersonalRecordMockup> PlaceholderPRs { get; set; }

    public ProfileViewModel()
    {
        UserPlaceholder = new UserMockup { Username = "John_Doe", Age = 21, Gender = "Male" };

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
                Value = 422,
                Type = 1
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