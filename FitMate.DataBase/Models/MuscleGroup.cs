using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitMate.Models;

[Table("MuscleGroups")]
public class MuscleGroup
{
    [Key]
    public int ID { get; set; }
    
    [Required]
    public string Name { get; set; }
}

public enum MuscleGroupType
{
    Abs,
    Back,
    Biceps,
    Cardio,
    Chest,
    Legs,
    Shoulders,
    Triceps
}