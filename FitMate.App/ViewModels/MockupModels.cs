namespace FitMate.ViewModels.Mockups;


public struct UserMockup
{
    public string Username { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }
}

public struct PersonalRecordMockup
{
    public string Name { get; set; }
    public Models.ExerciseSet ExerciseSet { get; set; }
}
public struct WorkoutMockup
{
    public DateTime Date { get; set; }
    public string MusclesWorked { get; set; }
    public string DateString => Date.ToString("dddd - dd/MM/yyyy");
}

public struct ExerciseMockup
{
    public string Name { get; set; }
    public Models.ExerciseSet ExerciseSet { get; set; }
    public bool IsPersonalRecord { get; set; }
    public MuscleGroup MuscleGroup { get; set; }
}

//TODO: write a wrapper to generate groups from a list of exercises.
public class ExerciseGroupMockup : List<ExerciseMockup> 
{
    public string Name { get; set; }

    public ExerciseGroupMockup(string name, List<ExerciseMockup> exercises) : base(exercises)
    {
        Name = name;
    }
}

public enum MuscleGroup
{
    Abdominal,
    Back,
    Biceps,
    Cardio,
    Chest,
    Legs,
    Shoulders,
    Triceps
}