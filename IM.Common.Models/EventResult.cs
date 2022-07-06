namespace IM.Common.Models;

public class EventResult : IResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public int StatusCode { get; set; }

    public static EventResult Failed(string? message = null)
    {
        return new EventResult { StatusCode = 400, Message = message };
    }
}
