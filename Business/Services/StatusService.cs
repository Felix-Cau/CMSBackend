using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Business.Services
{
    public class StatusService(IStatusRepository statusRepository, IMemoryCache cache) : IStatusService
    {
        private readonly IStatusRepository _statusRepository = statusRepository;
        private readonly IMemoryCache _cache = cache;
        private const string _cacheKey_All = "Project_All";

        public async Task<ServiceResult<IEnumerable<StatusDto>>> GetStatusesAsync()
        {
            if (_cache.TryGetValue(_cacheKey_All, out IEnumerable<StatusDto> cachedItems))
                return ServiceResult<IEnumerable<StatusDto>>.Ok(cachedItems, "Ok");

            var statusDtoList = await SetCache();
            return statusDtoList is not null && statusDtoList.Any()
                ? ServiceResult<IEnumerable<StatusDto>>.Ok(statusDtoList, "Ok")
                : ServiceResult<IEnumerable<StatusDto>>.Failed([], "An unexpected error occured");
        }

        public async Task<ServiceResult<StatusDto>> GetStatusByStatusNameAsync(string statusName)
        {
            StatusDto statusDto = new();

            if (_cache.TryGetValue(_cacheKey_All, out IEnumerable<StatusDto>? cachedItems))
            {
                statusDto = cachedItems.FirstOrDefault(s => s.StatusName == statusName);
                if (statusDto is not null)
                    return ServiceResult<StatusDto>.Ok(statusDto, "Ok");
            }

            var tempStatusEntity = await _statusRepository.GetAsync(s => s.StatusName == statusName);
            if (tempStatusEntity is null)
                return ServiceResult<StatusDto>.NotFound(new StatusDto(), "Not found");

            statusDto = StatusFactory.ToModel(tempStatusEntity);
            if (statusDto is null)
                return ServiceResult<StatusDto>.Failed(new StatusDto(), "An unexpected error occured");

            await SetCache();
            return ServiceResult<StatusDto>.Ok(statusDto, "Ok");
        }

        public async Task<IEnumerable<StatusDto>> SetCache()
        {
            _cache.Remove(_cacheKey_All);
            var statusEntityList = await _statusRepository.GetAllAsync(sortByExpression: x => x.Id);
            var statusDtoList = statusEntityList.Select(StatusFactory.ToModel);

            if (statusDtoList.Any(entity => entity is null))
                return [];

            _cache.Set(_cacheKey_All, statusDtoList, TimeSpan.FromDays(5));
            return statusDtoList!;
        }
    }
}
