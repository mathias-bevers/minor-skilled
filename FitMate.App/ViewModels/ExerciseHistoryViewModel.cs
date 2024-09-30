using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FitMate.ViewModels.Mockups;

namespace FitMate.ViewModels;

public class ExerciseHistoryViewModel : ObservableObject
{
    public ObservableCollection<ExerciseGroupMockup> Exercises { get; set; }
    public PersonalRecordMockup PR { get; set; }

    public ExerciseHistoryViewModel()
    {
        PR = new PersonalRecordMockup
        {
            ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 12, 10)
        };

        Exercises = new ObservableCollection<ExerciseGroupMockup>([
            new ExerciseGroupMockup(DateTime.Today.ToShortDateString(), [
                new ExerciseMockup { ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 10, 10) },
                new ExerciseMockup { ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 10, 10) },
                new ExerciseMockup
                {
                    ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 12, 10),
                    IsPersonalRecord = true
                }
            ]),
            
            new ExerciseGroupMockup(DateTime.Today.AddDays(-3).ToShortDateString(), [
                new ExerciseMockup { ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 10, 10) },
                new ExerciseMockup { ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 10, 10) },
                new ExerciseMockup { ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 10, 10) },
            ]),
            
            new ExerciseGroupMockup(DateTime.Today.AddDays(-5).ToShortDateString(), [
                new ExerciseMockup { ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 10, 10) },
                new ExerciseMockup { ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 10, 10) },
                new ExerciseMockup { ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 10, 10) },
            ]),
            
            new ExerciseGroupMockup(DateTime.Today.AddDays(-7).ToShortDateString(), [
                new ExerciseMockup { ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 8, 10) },
                new ExerciseMockup { ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 8, 10) },
                new ExerciseMockup { ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 8, 10) },
            ])
        ]);
    }
}