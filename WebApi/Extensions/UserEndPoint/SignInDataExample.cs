using Authentication.Models;
using Swashbuckle.AspNetCore.Filters;

namespace WebApi.Extensions.UserEndPoint
{
    public class SignInDataExample : IExamplesProvider<SignInForm>
    {
        public SignInForm GetExamples() => new()
        {
            Email = "john.doe@domain.com",
            Password = "BytMig123!"
        };
    }
}
