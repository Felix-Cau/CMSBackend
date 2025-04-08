using Domain.Models;
using Swashbuckle.AspNetCore.Filters;

namespace WebApi.Extensions.UserEndPoint
{
    public class UserLIstDataExample : IExamplesProvider<IEnumerable<AppUserDto>>
    {
        public IEnumerable<AppUserDto> GetExamples()
        {
            return new List<AppUserDto>
            {
                new AppUserDto
                {
                    //AI generated
                    Id = "3619fb70-65e6-47a4-96e8-be4821fbd409",
                    ImageUrl = "23f24c20-8fa1-48f8-8b09-75b00a0c6baa-2.jpg",
                    FirstName = "Alice",
                    LastName = "Johnson",
                    JobTitle = "Software Engineer",
                    Role = "Admin",
                    Address = "123 Tech Street",
                    PostalCode = "12345",
                    City = "Techville"
                },
                new AppUserDto
                {
                    //AI generated
                    Id = "23f24c20-8fa1-48f8-8b09-75b00a0c6baa",
                    ImageUrl = "23f24c20-8fa1-48f8-8b09-75b00a0c6baa-56.jpg",
                    FirstName = "Bob",
                    LastName = "Smith",
                    JobTitle = "Product Manager",
                    Role = "User",
                    Address = "456 Product Road",
                    PostalCode = "67890",
                    City = "ManageTown"
                }
            };
        }
    }
}
