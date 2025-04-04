using Business.Models;
using Data.Entities;

namespace Business.Factories
{
    public class StatusFactory
    {
        public static StatusDto? ToModel(StatusEntity entity)
        {
            if (entity is null) 
                return null;

            StatusDto statusDto = new()
            {
                Id = entity.Id,
                StatusName = entity.StatusName
            };

            return statusDto;
        }
    }
}
