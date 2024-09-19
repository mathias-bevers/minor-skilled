namespace FitMate.ViewModels;


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
}