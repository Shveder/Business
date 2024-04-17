using System.ComponentModel.DataAnnotations;

namespace Business_monitoring.Models;

public class Offer : BaseEntity
{
    [Key] public Guid Id { get; set; }
    [Required] public UserModel User { get; set; }
    [Required] public Business Business { get; set; }
    [Required] public int NumberOfShares { get; set; }
    [Required] public double PriceForShare { get; set; }
}