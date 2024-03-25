using System.ComponentModel.DataAnnotations;

namespace Business_monitoring.Models;

public class RecentPricesOfBusiness : BaseEntity
{
    [Key] public Guid Id { get; set; }
    [Required] public double Price { get; set; }
    [Required] public Business Business { get; set; }

}