using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FitMate.Models;
using FitMate.ViewModels.Mockups;

namespace FitMate.ViewModels;

public class ExerciseLeaderboardViewModel : ObservableObject
{
    public ObservableCollection<LeaderboardEntry> PRs { get; set; }

    public ExerciseLeaderboardViewModel()
    {
        PRs = new ObservableCollection<LeaderboardEntry>();
        
        List<PersonalRecordMockup> dbMockup =
        [
            new() { Name = "John_Doe", ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 12, 10) },
            new() { Name = "John_Doe", ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 12, 12) },
            new() { Name = "John_Doe", ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 10, 12) },
            new() { Name = "John_Doe", ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 10, 10) },
            new() { Name = "John_Doe", ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 14, 12) },
            new() { Name = "John_Doe", ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 8, 14) }
        ];

        dbMockup.Sort((a, b) => b.ExerciseSet.CompareTo(a.ExerciseSet));

        for (int i = 0; i < dbMockup.Count; ++i)
        {
            PRs.Add(new LeaderboardEntry(i + 1, dbMockup[i]));
        }
    }
}

public struct LeaderboardEntry
{
    public int Placement { get; }
    public PersonalRecordMockup Record { get; }

    public LeaderboardEntry(int placement, PersonalRecordMockup record)
    {
        Placement = placement;
        Record = record;
    }
}