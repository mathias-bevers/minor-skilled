using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitMate.Models;

[Table("MeasurementTypes")]
public class MeasurementType
{
    [Key]
    public int ID { get; set; }

    [Required]
    public string Name { get; set; }
}

public enum Measurement
{
    KilosPerRepetition,
    MetersPerSecond,
}