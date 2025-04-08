using Domain.Models;
using Swashbuckle.AspNetCore.Filters;

namespace WebApi.Extensions.UserEndPoint
{
    public class ServiceResultErrorExample : IExamplesProvider<ServiceResult>
    {
        public ServiceResult GetExamples() => new()
        {
            Message = "Internal server error"
        };
    }
}
