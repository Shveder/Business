namespace Business_monitoring.Exceptions;

public class IncorrectDataException : Exception
{
    public IncorrectDataException(string? message) : base(message)
    {
    }
}