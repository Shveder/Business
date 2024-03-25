using System.ComponentModel.DataAnnotations;
using Business_monitoring.Models.Interfaces;

namespace Business_monitoring.Models;

public class GainsOfCompany : IModels
{
    [Key] public Guid Id { get; set; }
    [Required] public string Year { get; set; }
    [Required] public double Gain { get; set; }
    [Required] public Business Business { get; set; }
    
}