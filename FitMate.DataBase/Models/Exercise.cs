using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitMate.Models;

[Table("Exercises")]
public class Exercise
{
    [Key]
    public int ID { get; set; }

    [Required]
    public int KgsOrMtr { get; set; }
    [Required]
    public int RepsOrSecs { get; set; }
    [NotMapped]
    public bool IsPR { get; set; }

    [Required]
    public int WorkoutID { get; set; }
    public Workout Workout { get; set; } = null!;
    
    [Required]
    public int ExerciseTypeID { get; set; }
    public ExerciseType ExerciseType { get; set; } = null!;

    [NotMapped]
    public DateTime Date { get; set; } = DateTime.MinValue;


    public override string ToString()
    {
        System.Text.StringBuilder sb = new();
        
        if (ExerciseType.MeasurementType == Measurement.KilosPerRepetition)
        {
            sb.Append(string.Concat(KgsOrMtr, " KGS - ", RepsOrSecs,  " REPS"));
        }
        else
        {
            TimeSpan ts = TimeSpan.FromSeconds(RepsOrSecs);
            sb.Append(string.Concat(KgsOrMtr, " MTR - "));
            sb.Append(string.Concat(Convert.ToInt32(ts.TotalMinutes), ':', ts.Seconds.ToString("00"), "MINS"));
        }
        
        return sb.ToString();
    }
}