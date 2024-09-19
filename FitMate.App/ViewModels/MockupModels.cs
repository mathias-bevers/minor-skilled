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
    public int Measurement { get; set; }
    public int Value { get; set; }
    public int Type { get; set; }

    public string FormattedString
    {
        get
        {
            if (Type == 0) { return $"{Measurement} KGS\t{Value} REPS"; }

            TimeSpan ts = TimeSpan.FromSeconds(Value);
            string formattedTime = $"{Convert.ToInt32(ts.TotalMinutes)}:{ts.Seconds:00}";
            return $"{Measurement} MTR {formattedTime}\tMINS";
        }
    }
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