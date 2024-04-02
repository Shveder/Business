using Business_monitoring.DTO;

namespace Business_monitoring.Services.Interfaces;

public interface IExpertService
{
    public Task AddExpertView(AddViewRequest request);
}