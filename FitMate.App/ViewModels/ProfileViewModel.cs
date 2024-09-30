using System.Collections.ObjectModel;
using FitMate.Models;
using FitMate.ViewModels.Mockups;

namespace FitMate.ViewModels;

public class ProfileViewModel
{
    public User? User { get; set; }
    
    public ObservableCollection<PersonalRecordMockup> PlaceholderPRs { get; set; }

    public ProfileViewModel()
    {
        using (DataBase.FitMateDB db = new())
        {
            User = db.Users.FirstOrDefault();
        }
        

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