using System.Linq.Expressions;

namespace Infrastructure.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression);
    Task<bool> AddAsync(TEntity entity);
    Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> expression);
    Task<IEnumerable<TEntity>> GetAllAsync(bool orderByDescending = false, Expression<Func<TEntity, object>>? sortByExpression = null, Expression<Func<TEntity, bool>>? filterByExpression = null, params Expression<Func<TEntity, object>>[] includes);
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> findByExpression, params Expression<Func<TEntity, object>>[] includes);
    Task<bool> UpdateAsync(TEntity entity);
}