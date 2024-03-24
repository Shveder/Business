using System.Linq.Expressions;
using Business_monitoring.Data;
using Business_monitoring.Models.Interfaces;
using Business_monitoring.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Business_monitoring.Repository;

public class DbRepository : IDbRepository
{
    private readonly Context _context;

    public DbRepository(Context context)
    {
        _context = context;
    }

    public IQueryable<T> Get<T>() where T : class, IModels
    {
        return _context.Set<T>().AsQueryable();
    }

    public IQueryable<T> Get<T>(Expression<Func<T, bool>> selector) where T : class, IModels
    {
        return _context.Set<T>().Where(selector).AsQueryable();
    }

    public async Task<Guid> Add<T>(T newEntity) where T : class, IModels
    {
        var entity = await _context.Set<T>().AddAsync(newEntity);
        Console.WriteLine(entity.Entity.Id);
        return entity.Entity.Id;
    }


    public async Task Delete<T>(Guid id) where T : class, IModels
    {
        var entity = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        if (entity != null)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task Update<T>(T entity) where T : class, IModels
    {
        await Task.Run(() => _context.Set<T>().Update(entity));
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public IQueryable<T> GetAll<T>() where T : class, IModels
    {
        return _context.Set<T>().AsQueryable();
    }
}