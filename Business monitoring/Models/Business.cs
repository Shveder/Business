using System.ComponentModel.DataAnnotations;

namespace Business_monitoring.Models;

public class Business : BaseEntity
{
    [Key] public Guid Id { get; set; }
    [Required] public string Name { get; set; }
    [Required] public double PriceOfCompany { get; set; }
    [Required] public int NumberOfShares { get; set; }
    [Required] public int PriceOfShare { get; set; }
    [Required] public string ExpertViewPrice { get; set; }
    [Required] public Company Company { get; set; }
}