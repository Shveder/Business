using System.ComponentModel.DataAnnotations;
using Business_monitoring.Models.Interfaces;

namespace Business_monitoring.Models;

public class RecentPricesOfBusiness : IModels
{
    [Key] public Guid Id { get; set; }
    [Required] public double Price { get; set; }
    [Required] public Business Business { get; set; }
    [Required] public DateTime Date { get; set; }
    
}