using Authentication.Models;
using Swashbuckle.AspNetCore.Filters;

namespace WebApi.Extensions.UserEndPoint
{
    public class SignUpDataExample : IExamplesProvider<SignUpForm>
    {
        public SignUpForm GetExamples() => new()
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@domain.com",
            Password = "BytMig123!",
        };
    }
}
