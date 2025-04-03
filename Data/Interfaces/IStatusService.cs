using Data.Entities;

namespace Business.Interfaces
{
    public interface IStatusService
    {
        Task<IEnumerable<StatusEntity>> GetStatusesAsync();
        Task<StatusEntity> GetStatusByStatusNameAsync(string statusName);
    }
}
