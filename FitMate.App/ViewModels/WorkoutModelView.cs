using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FitMate.ViewModels.Mockups;

namespace FitMate.ViewModels;

public class WorkoutModelView : ObservableObject
{
    public ObservableCollection<ExerciseGroupMockup> Exercises { get; set; }

    public WorkoutModelView()
    {
        Exercises = new ObservableCollection<ExerciseGroupMockup>([
            new ExerciseGroupMockup("Treadmill", [
                new ExerciseMockup
                {
                    ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.MeterMinutes, 1320, 421),
                    Name = "Treadmill",
                    IsPersonalRecord = true,
                    MuscleGroup = MuscleGroup.Cardio
                }
            ]),
            new ExerciseGroupMockup("Biceps", [
                new ExerciseMockup
                {
                    ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 12, 10),
                    Name = "Hammer Curl",
                    IsPersonalRecord = false,
                    MuscleGroup = MuscleGroup.Biceps
                },
                new ExerciseMockup
                {
                    ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 12, 10),
                    Name = "Hammer Curl",
                    IsPersonalRecord = false,
                    MuscleGroup = MuscleGroup.Biceps
                },
                new ExerciseMockup
                {
                    ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 14, 10),
                    Name = "Hammer Curl",
                    IsPersonalRecord = true,
                    MuscleGroup = MuscleGroup.Biceps
                }
            ]),
            new ExerciseGroupMockup("Incline Bench Press", [
                new ExerciseMockup
                {
                    ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 55, 10),
                    Name = "Incline Bench Press",
                    IsPersonalRecord = false,
                    MuscleGroup = MuscleGroup.Chest
                },
                new ExerciseMockup
                {
                    ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 60, 10),
                    Name = "Incline Bench Press",
                    IsPersonalRecord = true,
                    MuscleGroup = MuscleGroup.Chest
                },
                new ExerciseMockup
                {
                    ExerciseSet = new ExerciseSet((int)ExerciseSet.SetType.KiloReps, 55, 10),
                    Name = "Incline Bench Press",
                    IsPersonalRecord = false,
                    MuscleGroup = MuscleGroup.Chest
                }
            ])
        ]);
    }
}

/*
,
*/