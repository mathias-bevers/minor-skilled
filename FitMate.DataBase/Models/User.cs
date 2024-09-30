using System.ComponentModel.DataAnnotations;

namespace FitMate.Models;

public class User
{
    [Key] public int ID { get; set; }
    
    public string UserName { get; set; } 
    
    public string DateOfBirth { get; set; }
    
    public ICollection<Workout> Workouts { get; }
    
    //public int Gender { get; set; } . TODO: add gender through enum
    
}