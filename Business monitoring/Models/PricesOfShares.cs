using System.ComponentModel.DataAnnotations;

namespace Business_monitoring.Models;

public class PricesOfShares : BaseEntity
{
    [Key] public Guid Id { get; set; }
    [Required] public double PriceOfShare { get; set; }
    [Required] public Business Business { get; set; }
}