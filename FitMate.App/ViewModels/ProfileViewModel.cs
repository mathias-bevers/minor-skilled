using System.Collections.ObjectModel;
using FitMate.ViewModels.Mockups;

namespace FitMate.ViewModels;

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
                ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 14, 12),
            },
            new PersonalRecordMockup()
            {
                Name = "Treadmill",
                ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.MeterMinutes, 1320, 422),
            },
            new PersonalRecordMockup()
            {
                Name = "Leg Press",
                ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 160, 10),
            }
        ]);
    }
}