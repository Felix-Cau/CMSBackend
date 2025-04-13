namespace Domain.Models
{
    public class ServiceResult
    {
        public bool Succeeded { get; set; }
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
        public bool? IsAdmin { get; set; }
        public string? AdminApiKey { get; set; }

        public static ServiceResult Ok(string? message = null) =>
            new() { Succeeded = true, StatusCode = 200, Message = message };

        public static ServiceResult AuthOk(string? token = null, bool? isAdmin = null, string? adminApiKey = null, string? message = null) =>
            new() { Succeeded = true, StatusCode = 200, Token = token, IsAdmin = isAdmin, AdminApiKey = adminApiKey, Message = message };

        public static ServiceResult Created(string? message = null) =>
            new() { Succeeded = true, StatusCode = 201, Message = message };

        public static ServiceResult BadRequest(string? message = "Invalid field(s)") =>
            new() { Succeeded = false, StatusCode = 400, Message = message };

        public static ServiceResult UnAuthorized(string? message = "Username or password is incorrect.") =>
            new() { Succeeded = false, StatusCode = 401, Message = message };

        public static ServiceResult NotFound(string? message = "Not found") =>
            new() { Succeeded = false, StatusCode = 404, Message = message };

        public static ServiceResult AlreadyExists(string? message = "Already exists") =>
            new() { Succeeded = false, StatusCode = 409, Message = message };

        public static ServiceResult Failed(string? message = "An unexpected error occured") =>
            new() { Succeeded = false, StatusCode = 500, Message = message };
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T? Result { get; set; }

        public static ServiceResult<T> Ok(T result, string? message) =>
            new() { Succeeded = true, StatusCode = 200, Message = message, Result = result };

        public static ServiceResult<T> SignInOk( T result, string? token = null, bool? isAdmin = null, string? adminApiKey = null, string? message = null) =>
            new() { Succeeded = true, StatusCode = 200, Token = token, IsAdmin = isAdmin, AdminApiKey = adminApiKey, Message = message, Result = result };

        public static ServiceResult<T> BadRequest(T result, string? message) =>
            new() { Succeeded = false, StatusCode = 400, Message = message, Result = result };

        public static ServiceResult<T> UnAuthorized(T result, string? message) =>
            new() { Succeeded = false, StatusCode = 401, Message = message, Result = result };

        public static ServiceResult<T> NotFound(T result, string? message) =>
            new() { Succeeded = false, StatusCode = 404, Message = message, Result = result};

        public static ServiceResult<T> Failed(T result, string? message) =>
            new() { Succeeded = false, StatusCode = 500, Message = message, Result = result };
    }
}
