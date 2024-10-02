using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace FitMate.Models;

[Table("Genders")]
public class Gender
{
    [Key]
    public int ID { get; set; }

    [Required]
    public string Value { get; set; }
}

public enum GenderType
{
    Woman,
    Man,
    Other,
    PreferNotToSay
}