using System.ComponentModel.DataAnnotations;

namespace FitMate.Models;

public class Exercise
{
    [Key]
    public int ID { get; set; }

    [Required]
    public int KgsOrMtr { get; set; }
    [Required]
    public int RepsOrSecs { get; set; }
    [Required]
    public bool IsPR { get; set; }

    [Required]
    public int WorkoutID { get; set; }
    public Workout Workout { get; set; } = null!;
    [Required]
    public int ExerciseTypeID { get; set; }
    public ExerciseType ExerciseType { get; set; } = null!;


    public override string ToString()
    {
        if (ExerciseType.MeasurementType == Measurement.KilosPerRepetition)
        {
            return $"{KgsOrMtr} KGS - {RepsOrSecs} REPS";
        }

        TimeSpan ts = TimeSpan.FromSeconds(RepsOrSecs);
        return $"{KgsOrMtr} MTR - {Convert.ToInt32(ts.TotalMinutes)}:{ts.Seconds:00} MINS";
    }
}