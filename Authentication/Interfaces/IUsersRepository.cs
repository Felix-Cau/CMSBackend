using System.Linq.Expressions;
using Authentication.Models;
using Domain.Models;

namespace Authentication.Interfaces;

public interface IUsersRepository
{
    Task<bool> ExistsAsync(SignUpForm form);

    Task<bool> ExistsAsync(NewAppUserForm form);

    Task<ServiceResult<IEnumerable<AppUserDto>>> GetAllAsync(bool orderByDescending = false,
        Expression<Func<AppUserEntity, object>>? sortByExpression = null,
        Expression<Func<AppUserEntity, bool>>? findByExpression = null,
        params Expression<Func<AppUserEntity, object>>[] includes);

    Task<ServiceResult<AppUserDto>> GetUserAsync(Expression<Func<AppUserEntity, bool>> findByExpression,
        params Expression<Func<AppUserEntity, object>>[] includes);
}