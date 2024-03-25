using System.ComponentModel.DataAnnotations;

namespace Business_monitoring.Models;

public class Deposits : BaseEntity
{
    [Key] public Guid Id { get; set; }
    [Required] public double SumOfDeposit { get; set; }
    [Required] public UserModel User { get; set; }
}