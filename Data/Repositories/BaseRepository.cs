using Data.Contexts;
using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Data.Repositories;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly DataContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    protected BaseRepository(DataContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public virtual async Task<RepositoryResult<TEntity>> AddAsync(TEntity entity)
    {
        if (entity == null)
            return new RepositoryResult<TEntity> { Success = false, StatusCode = 400, Error = "Entity can't be null." };

        try
        {
            var result = _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return new RepositoryResult<TEntity> { Success = true, StatusCode = 201, Result = result.Entity };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new RepositoryResult<TEntity> { Success = false, StatusCode = 500, Error = ex.Message };
        }
    }

    public virtual async Task<RepositoryResult<IEnumerable<TEntity>>> GetAllAsync(bool orderDescending = false, Expression<Func<TEntity, object>>? sortBy = null, Expression<Func<TEntity, bool>>? where = null, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        if (where != null)
            query = query.Where(where);

        if (includes != null && includes.Length != 0)
            foreach (var include in includes)
                query.Include(include);

        if (sortBy != null)
            query = orderDescending
                ? query.OrderByDescending(sortBy)
                : query.OrderBy(sortBy);

        var entities = await query.ToListAsync();
        return entities.IsNullOrEmpty()
            ? new RepositoryResult<IEnumerable<TEntity>> { Success = false, StatusCode = 404, Error = "No entities found." }
            : new RepositoryResult<IEnumerable< TEntity >> { Success = true, StatusCode = 200, Result = entities };
    }

    public virtual async Task<RepositoryResult<TEntity>> GetAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        if (includes != null && includes.Length != 0)
            foreach (var include in includes)
                query.Include(include);

        var entity = await query.FirstOrDefaultAsync(where);

        return entity == null
            ? new RepositoryResult<TEntity> { Success = false, StatusCode = 404, Error = "Entity not found.", Result = null }
            : new RepositoryResult<TEntity> { Success = true, StatusCode = 200, Result = entity };
    }

    public virtual async Task<RepositoryResult<bool>> UpdateAsync(TEntity entity)
    {
        if (entity == null)
            return new RepositoryResult<bool> { Success = false, StatusCode = 400, Error = "Entity can't be null." };

        try
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return new RepositoryResult<bool> { Success = true, StatusCode = 201 };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new RepositoryResult<bool> { Success = false, StatusCode = 500, Error = ex.Message };
        }
    }

    public virtual async Task<RepositoryResult<bool>> DeleteAsync(TEntity entity)
    {
        if (entity == null)
            return new RepositoryResult<bool> { Success = false, StatusCode = 400, Error = "Entity can't be null." };

        try
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return new RepositoryResult<bool> { Success = true, StatusCode = 201 };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new RepositoryResult<bool> { Success = false, StatusCode = 500, Error = ex.Message };
        }
    }

    public virtual async Task<RepositoryResult<bool>> AnyAsync(Expression<Func<TEntity, bool>> expression)
    {
        if (expression == null)
            return new RepositoryResult<bool> { Success = false, StatusCode = 400, Error = "Expression can't be null." };

        var exists = await _dbSet.AnyAsync(expression);
        return exists
            ? new RepositoryResult<bool> { Success = true, StatusCode = 200 }
            : new RepositoryResult<bool> { Success = false, StatusCode = 404, Error = "Entity not found." };
    }
}
