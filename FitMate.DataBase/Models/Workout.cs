using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitMate.Models;

[Table("Workouts")]
public class Workout
{
    [Key]
    public int ID { get; set; }

    [Required]
    public string CreatedOn { get; set; }

    [Required]
    public int UserID { get; set; }
    public User User { get; set; } = null!;
    public ICollection<Exercise> Exercises { get; }

    [NotMapped]
    public string DateString => DateTime
        .ParseExact(CreatedOn, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)
        .ToString("dddd - dd/MM/yyyy");
    
    [NotMapped]
    public string MusclesWorked { get; set; }
}