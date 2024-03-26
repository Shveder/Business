using Business_monitoring.Models.Interfaces;

namespace Business_monitoring.Models;

public class BaseEntity : IModels
{
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime? DateUpdated { get; set; }
    public Guid Id { get; set; }
}