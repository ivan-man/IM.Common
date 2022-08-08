using Grpc.Core;
using Grpc.Core.Interceptors;
using IM.Common.Models;
using Mapster;

namespace IM.Common.Grpc;

public class ResponseInterceptor : Interceptor
{
    public ResponseInterceptor()
    {
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        var call = continuation(request, context);
        return new AsyncUnaryCall<TResponse>(
            HandleResponse(call.ResponseAsync),
            call.ResponseHeadersAsync,
            call.GetStatus,
            call.GetTrailers,
            call.Dispose);
    }

    public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var call = continuation(request, context);
        return HandleResponse(call);
    }

    private static async Task<TResponse> HandleResponse<TResponse>(Task<TResponse> callResponseAsync)
        where TResponse : class
    {
        var isResult = typeof(IResult).IsAssignableTo(typeof(TResponse));
        try
        {
            return await callResponseAsync;
        }
        catch (RpcException e)
        {
            var result = e.Status.StatusCode switch
            {
                StatusCode.Aborted => new { Success = false, Message = "Request aborted", StatusCode = 422 },
                StatusCode.OK => new { Success = true, Message = "", StatusCode = 200 },
                StatusCode.Cancelled => new { Success = false, Message = "Request cancelled", StatusCode = 400 },
                StatusCode.Unknown => new { Success = false, Message = "Unknown request handled", StatusCode = 400 },
                StatusCode.InvalidArgument => new
                {
                    Success = false, Message = "Invalid input arguments", StatusCode = 400
                },
                StatusCode.DeadlineExceeded => new
                {
                    Success = false, Message = "Invalid request length", StatusCode = 411
                },
                StatusCode.NotFound => new { Success = false, Message = "Not found", StatusCode = 404 },
                StatusCode.AlreadyExists => new { Success = false, Message = "Already exists", StatusCode = 400 },
                StatusCode.PermissionDenied => new
                {
                    Success = false, Message = "Permission denied", StatusCode = 403
                },
                StatusCode.Unauthenticated => new { Success = false, Message = "Unauthenticated", StatusCode = 401 },
                StatusCode.ResourceExhausted => new
                {
                    Success = false, Message = "Server is too busy", StatusCode = 509
                },
                StatusCode.FailedPrecondition => new { Success = false, Message = "Bad request", StatusCode = 400 },
                StatusCode.OutOfRange => new { Success = false, Message = "Bad request", StatusCode = 400 },
                StatusCode.Unimplemented => new
                {
                    Success = false, Message = "Method is not implemented",
                    StatusCode = 501
                },
                StatusCode.Internal => new
                {
                    Success = false, Message = "Internal server error", StatusCode = 500
                },
                StatusCode.Unavailable => new
                {
                    Success = false, Message = "Service in not reachable", StatusCode = 503
                },
                StatusCode.DataLoss => new
                {
                    Success = false, Message = "Got some data loss", StatusCode = 500
                },
                _ => new { Success = false, Message = "Invalid request handled", StatusCode = 500 }
            };
            return result.Adapt<TResponse>();
        }
        catch
        {
            if (!isResult)
                throw;

            return Result.Internal("Internal server error").Adapt<TResponse>();
        }
    }
}
