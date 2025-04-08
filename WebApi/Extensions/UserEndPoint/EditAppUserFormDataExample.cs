using System.ComponentModel.DataAnnotations;
using Domain.Models;
using Swashbuckle.AspNetCore.Filters;

namespace WebApi.Extensions.UserEndPoint
{
    public class EditAppUserFormDataExample : IExamplesProvider<EditAppUserForm>
    {
        public EditAppUserForm GetExamples() => new()
        {
            Id = "c87c2347-4747-48ea-b6b5-63472f151821",
            FirstName = "John",
            LastName = "Doe",
            JobTitle = "CTO",
            PhoneNumber = "0700345345",
            Role = "User",
            Address = "Falkvägen 1",
            PostalCode = "11234",
            City = "Stockholm"
        };
    }
}