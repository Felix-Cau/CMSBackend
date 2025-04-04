using Business.Models;
using Data.Entities;
using Domain.Models;

namespace Business.Interfaces
{
    public interface IStatusService
    {
        Task<ServiceResult<IEnumerable<StatusDto>>> GetStatusesAsync();
        Task<ServiceResult<StatusDto>> GetStatusByStatusNameAsync(string statusName);
    }
}
