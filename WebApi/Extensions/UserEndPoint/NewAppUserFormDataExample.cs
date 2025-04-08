using System.ComponentModel.DataAnnotations;
using Domain.Models;
using Swashbuckle.AspNetCore.Filters;

namespace WebApi.Extensions.UserEndPoint
{
    public class NewAppUserFormDataExample : IExamplesProvider<NewAppUserForm>
    {
        public NewAppUserForm GetExamples() => new()
        {
            Email = "john.doe@domain.com",
            FirstName = "john",
            LastName = "doe",
            PhoneNumber = "0700123456",
            JobTitle = "CTO",
            Role = "User",
            Address = "Vannasvägen 1",
            PostalCode = "23376",
            City = "Falköping"
        };
    }
}