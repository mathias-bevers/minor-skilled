using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FitMate.Models;

public class Gender
{
    [Key] public int ID { get; set; }
    [NotNull] public string Value { get; set; }
}