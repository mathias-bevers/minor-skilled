using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitMate.Models;

[Table("ExerciseTypes")]
public class ExerciseType
{
    [Key, Required]
    public string Name { get; set; }
    
    [Required]
    public int MuscleGroupID { get; set; }
    public MuscleGroup MuscleGroup { get; set; }
   
    [Required]
    public int MeasurementTypeID { get; set; }
    public MeasurementType Measurement { get; set; }

    [NotMapped]
    public MuscleGroupType MuscleGroupType
    {
        get => (MuscleGroupType)(MuscleGroupID - 1);
        set => MuscleGroupID = (int)(value + 1);
    }

    [NotMapped]
    public Measurement MeasurementType
    {
        get => (Measurement)(MeasurementTypeID - 1);
        set => MeasurementTypeID = (int)(value + 1);
    }
}