using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitMate.Models;

[Table("Users")]
public class User
{
    [Key]
    public int ID { get; set; }

    [Required]
    public string UserName { get; set; }
    [Required]
    public string DateOfBirth { get; set; }
    [Required]
    public bool SharePR { get; set; }= false;

    [ForeignKey("GenderID")]
    public int GenderID { get; set; }
    public Gender Gender { get; set; } = null!; //TODO: look at if this is necessary.
  
    public ICollection<Workout> Workouts { get; } = [];
    
    public ICollection<Follow> Following { get; } = [];

    [NotMapped]
    public GenderType GenderType
    {
        get => (GenderType)(GenderID - 1);
        set => GenderID = (int)(value + 1);
    }

    [NotMapped]
    public int Age
    {
        get
        {
            if (string.IsNullOrEmpty(DateOfBirth)) { return -1; }
            
            DateTime dob = DateTime.ParseExact(DateOfBirth, "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture);
            
            int age = DateTime.Today.Year - dob.Year;
            if (DateTime.Today.DayOfYear < dob.DayOfYear) { --age; }

            return age;
        }
    }
}