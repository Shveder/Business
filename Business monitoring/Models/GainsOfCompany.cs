using System.ComponentModel.DataAnnotations;

namespace Business_monitoring.Models;

public class GainsOfCompany : BaseEntity
{
    [Key] public Guid Id { get; set; }
    [Required] public string Year { get; set; }
    [Required] public double Gain { get; set; }
    [Required] public Business Business { get; set; }
    
}