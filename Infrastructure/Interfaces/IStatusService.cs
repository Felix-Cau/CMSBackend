using Infrastructure.Data.Entities;

namespace Infrastructure.Interfaces
{
    public interface IStatusService
    {
        Task<IEnumerable<StatusEntity>> GetStatusesAsync();
        Task<StatusEntity> GetStatusByStatusNameAsync(string statusName);
    }
}
