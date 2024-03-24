namespace Business_monitoring.DTO;

public class AuthorizationException : Exception
{
    public AuthorizationException(string? message) : base(message)
    {
    }
}