using Authentication.Contexts;
using Authentication.Factories;
using Authentication.Handlers;
using Authentication.Interfaces;
using Authentication.Models;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Authentication.Repositories
{
    public class UsersRepository(UserDbContext context, RoleHandler roleHandler) : IUsersRepository
    {
        private readonly UserDbContext _context = context;
        private readonly RoleHandler _roleHandler = roleHandler;

        public virtual async Task<bool> ExistsAsync(SignUpForm form)
        {
            return await _context.Users.AnyAsync(x => x.Email == form.Email);
        }

        public virtual async Task<bool> ExistsAsync(NewAppUserForm form)
        {
            return await _context.Users.AnyAsync(x => x.Email == form.Email);
        }

        public virtual async Task<ServiceResult<IEnumerable<AppUserDto>>> GetAllAsync(bool orderByDescending = false,
            Expression<Func<AppUserEntity, object>>? sortByExpression = null,
            Expression<Func<AppUserEntity, bool>>? findByExpression = null,
            params Expression<Func<AppUserEntity, object>>[] includes)
        {
            IQueryable<AppUserEntity> query = _context.Set<AppUserEntity>();

            if (findByExpression is not null)
                query = query.Where(findByExpression);

            if (includes is not null && includes.Length > 0)
                foreach (var include in includes)
                    query = query.Include(include);

            if (sortByExpression is not null)
                query = orderByDescending
                    ? query.OrderByDescending(sortByExpression)
                    : query.OrderBy(sortByExpression);

            var entities = await query.ToListAsync();

            List<AppUserDto> returnList = [];

            try
            {
                foreach (var entity in entities)
                {
                    var role = await _roleHandler.GetRoleAsync(entity);
                    var appUserDto = UserFactory.ToModel(entity, role);
                    returnList.Add(appUserDto);
                }
                return ServiceResult<IEnumerable<AppUserDto>>.Ok(returnList.AsEnumerable(), "Users fetched.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return ServiceResult<IEnumerable<AppUserDto>>.Failed(returnList.AsEnumerable(), "Couldn't fetch users.");
            }
        }

        public async Task<ServiceResult<AppUserDto>> GetUserAsync(Expression<Func<AppUserEntity, bool>> findByExpression, 
            params Expression<Func<AppUserEntity, object>>[] includes)
        {
            IQueryable<AppUserEntity> query = _context.Set<AppUserEntity>();

            if (includes is not null && includes.Length > 0)
                foreach (var include in includes)
                    query = query.Include(include);

            var entity = await query.FirstOrDefaultAsync(findByExpression);
            if (entity is null)
                return ServiceResult<AppUserDto>.NotFound(new AppUserDto(), "User not found.");

            var role = await _roleHandler.GetRoleAsync(entity!);
            if (role is null)
                return ServiceResult<AppUserDto>.Failed(new AppUserDto(), "Could not get user role and aborted.");

            var returnUserDto = UserFactory.ToModel(entity!, role);
            return (returnUserDto is null)
                    ? ServiceResult<AppUserDto>.Failed(returnUserDto!, "Could not create DTO of user")
                    : ServiceResult<AppUserDto>.Ok(returnUserDto!, "User retrieved successfully.");
        }
    }
}