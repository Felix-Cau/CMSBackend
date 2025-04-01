using Infrastructure.Data.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Services
{
    public class StatusService(IStatusRepository statusRepository) : IStatusService
    {
        private readonly IStatusRepository _statusRepository = statusRepository;

        public async Task<IEnumerable<StatusEntity>> GetStatusesAsync()
        {
            var entites = await _statusRepository.GetAllAsync(sortByExpression: x => x.Id);
            var statuses = entites.Select(entity => new StatusEntity()
            {
                Id = entity.Id,
                StatusName = entity.StatusName
            });

            return statuses;
        }

        public async Task<StatusEntity> GetStatusByStatusNameAsync(string statusName)
        {
            var entity = await _statusRepository.GetAsync(x => x.StatusName == statusName);
            return entity is null ? null! : new StatusEntity
            {
                Id = entity.Id,
                StatusName = entity.StatusName
            };
        }
    }
}
