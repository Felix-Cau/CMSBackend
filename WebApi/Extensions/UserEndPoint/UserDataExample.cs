using Domain.Models;
using Swashbuckle.AspNetCore.Filters;

namespace WebApi.Extensions.UserEndPoint
{
    public class UserDataExample : IExamplesProvider<AppUserDto>
    {
        public AppUserDto GetExamples() => new()
        {
            //AI generated
            Id = "123e4567-e89b-12d3-a456-426614174000",
            ImageUrl = "123e4567-e89b-12d3-a456-426614174000-1.jpg",
            FirstName = "John",
            LastName = "Doe",
            JobTitle = "Software Developer",
            Role = "Admin",
            Address = "123 Tech Street",
            PostalCode = "98765",
            City = "Codeville"
        };
    }
}
