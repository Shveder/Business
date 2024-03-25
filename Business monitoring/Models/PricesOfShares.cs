using System.ComponentModel.DataAnnotations;
using Business_monitoring.Models.Interfaces;

namespace Business_monitoring.Models;

public class PricesOfShares : IModels
{
    [Key] public Guid Id { get; set; }
    [Required] public double PriceOfShare { get; set; }
    [Required] public DateTime Date { get; set; }
    [Required] public Business Business { get; set; }
}