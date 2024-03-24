using System.Linq.Expressions;
using Business_monitoring.Models.Interfaces;

namespace Business_monitoring.Repository.Interfaces
{
    public interface IDbRepository
    {
        IQueryable<T> Get<T>(Expression<Func<T, bool>> selector) where T : class, IModels;
        IQueryable<T> Get<T>() where T : class, IModels;
        IQueryable<T> GetAll<T>() where T : class, IModels;
        Task<Guid> Add<T>(T newEntity) where T : class, IModels;
        Task Delete<T>(Guid entity) where T : class, IModels;
        Task Update<T>(T entity) where T : class, IModels;
        Task<int> SaveChangesAsync();
    }
}