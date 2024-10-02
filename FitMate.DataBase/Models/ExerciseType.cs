using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitMate.Models;

[Table("ExercisesTypes")]
public class ExerciseType
{
    [Key]
    public int ID { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public int MuscleGroupID { get; set; }
    public MuscleGroup MuscleGroup { get; set; }
    [Required]
    public int MeasurementTypeID { get; set; }
    public MeasurementType Measurement { get; set; }
}