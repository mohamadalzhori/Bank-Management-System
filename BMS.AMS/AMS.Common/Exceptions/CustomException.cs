using System.Net;

namespace BMS.Common.Exceptions;

public class CustomException: Exception
{
    public int StatusCode { get;}
    public CustomException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = (int)statusCode;
    }
}