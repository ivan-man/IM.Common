using Microsoft.AspNetCore.Mvc;

namespace IM.Common.Models.Mvc.Core.Response;

public static class ResultExtensions
{
    public static ObjectResult HttpResponse<T>(this IResult<T> result)
    {
        return new ObjectResult(result.Success ? result.Data : result.Message)
        {
            StatusCode = result.StatusCode,
        };
    }
    
    public static ObjectResult HttpResponse(this IResult result)
    {
        return new ObjectResult(result.Success ? null : result.Message)
        {
            StatusCode = result.StatusCode
        };
    }
}
