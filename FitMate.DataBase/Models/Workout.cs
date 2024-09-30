using System.ComponentModel.DataAnnotations;

namespace FitMate.Models;

public class Workout
{
    [Key] public int ID { get; set; }
    
    public string CreatedOn { get; set; }
    
    public int UserID { get; set; }
    public User User { get; set; }
}