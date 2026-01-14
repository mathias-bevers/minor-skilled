using CommunityToolkit.Mvvm.ComponentModel;

namespace FitMate.ViewModels.Mockups;


public struct PersonalRecordMockup
{
    public string Name { get; set; }
    public ExerciseSet ExerciseSet { get; set; }
}

public struct ExerciseSet : IComparable<ExerciseSet>
{
    public enum SetType { KiloReps, MeterMinutes }

    public SetType Type { get; set; }
    public int Measurement { get; set; }
    public int Value { get; set; }

    public string FormattedString
    {
        get
        {
            if (Type == SetType.KiloReps) { return $"{Measurement} KGS \t{Value} REPS"; }

            TimeSpan ts = TimeSpan.FromSeconds(Value);
            return $"{Measurement} MTR \t{Convert.ToInt32(ts.TotalMinutes)}:{ts.Seconds:00}";
        }
    }

    public ExerciseSet(int type, int measurement, int value)
    {
        Type = (SetType)type;
        Measurement = measurement;
        Value = value;
    }

    public int CompareTo(ExerciseSet other)
    {
        int total = Measurement * Value;
        int otherTotal = other.Measurement * other.Value;

        if (total == otherTotal) { return 0; }

        if (total > otherTotal) { return 1; }

        return -1;
    }
}