namespace CampusEcomSystemMini.Application.Exceptions;

public class AppException : Exception
{
    public AppException(
        AppErrorType errorType,
        string message)
        : base(message)
    {
        ErrorType = errorType;
    }

    public AppErrorType ErrorType { get; }
}