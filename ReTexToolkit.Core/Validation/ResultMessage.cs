using ReTexToolkit.Core.Exceptions;

namespace ReTexToolkit.Core.Validation;

/// <summary>
///     A class to hold the result of a message. Can be created using either an exception or an enum.
/// </summary>
public class ResultMessage
{
    public ResultMessage()
    {
    }

    public ResultMessage(Exception ex)
    {
        Code = ex.GetFormattedErrorCode();
        Message = ex.Message;
        Reference = ex.GetReference();
        StackTrace = ex.StackTrace;
        Exception = ex;
    }

    public ResultMessage(string message)
    {
        Reference = "UNREFERENCED_MESSAGE";
        Message = message;
    }

    public string? Code { get; init; }
    public string? Reference { get; init; }
    public string? Message { get; init; }
    public string? StackTrace { get; set; }
    private Exception? Exception { get; }

    public void Throw() => throw (Exception ?? new Exception(Message));

    public bool IsExceptionOfType<TException>() where TException : Exception => Exception is TException;
}