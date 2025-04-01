using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
using Infrastructure.Data.Contexts;

namespace Infrastructure.Data.Repositories;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly AlphaDbContext _context;
    protected readonly DbSet<TEntity> _table;

    protected BaseRepository(AlphaDbContext context)
    {
        _context = context;
        _table = _context.Set<TEntity>();
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await _table.AnyAsync(expression);
    }


    public virtual async Task<bool> AddAsync(TEntity entity)
    {
        if (entity is null)
            return false;

        try
        {
            _table.Add(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }

    public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> expression)
    {
        var entity = await _table.FirstOrDefaultAsync(expression);
        if (entity == default)
            return false;

        try
        {
            _table.Remove(entity);
            await _context.SaveChangesAsync();
            return !await _table.AnyAsync(expression);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }

    public virtual async Task<bool> DeleteManyAsync(Expression<Func<TEntity, bool>> expression)
    {
        var entity = await _table.Where(expression).ToListAsync();
        if (entity == default)
            return false;

        try
        {
            _table.RemoveRange(entity);
            await _context.SaveChangesAsync();
            return !await _table.AnyAsync(expression);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }


    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(bool orderByDescending = false, Expression<Func<TEntity, object>>? sortByExpression = null, 
        Expression<Func<TEntity, bool>>? filterByExpression = null, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _table;

        // filter som gör att vi kan hämta alla som är av en viss status (ex. COMPLETED)
        if (filterByExpression is not null)
            query = query.Where(filterByExpression);

        // inludes inkluderar all olika tabeller som jag vill ha med (ex. .Include(x => x.User)
        if (includes != null && includes.Length != 0)
            foreach (var include in includes)
                query = query.Include(include);

        // sortByExpression hanterar sorteringen av listan, ASC eller DESC och fält (ex. OrderBy(x => x.Created))
        if (sortByExpression is not null)
            query = orderByDescending
                ? query.OrderByDescending(sortByExpression)
                : query.OrderBy(sortByExpression);

        return await query.ToListAsync();
    }

    public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> findByExpression, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _table;

        if (includes is not null && includes.Length != 0)
            foreach (var include in includes)
                query = query.Include(include);

        var entity = await query.FirstOrDefaultAsync(findByExpression);
        return entity ?? null!;
    }

    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        if (entity is null)
            return false;

        try
        {
            _table.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }
}
