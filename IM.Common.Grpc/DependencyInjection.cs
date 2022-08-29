using Grpc.Net.ClientFactory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.ClientFactory;
using GrpcClientFactory = ProtoBuf.Grpc.Client.GrpcClientFactory;

namespace IM.Common.Grpc;

public static class DependencyInjection
{
    public static IServiceCollection AddGrpcService<T>(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName,
        bool useDefaultInterceptor = true,
        params InterceptorRegistration[] interceptors) where T : class
    {
        GrpcClientFactory.AllowUnencryptedHttp2 = true;
        var serviceUrl = configuration.GetValue<string>(sectionName);
        services.AddScoped<ResponseInterceptor>();

        return services
            .AddCodeFirstGrpcClient<T>(opts =>
            {
                if (useDefaultInterceptor)
                {
                    opts.InterceptorRegistrations.Add(new InterceptorRegistration(InterceptorScope.Channel,
                        (sp) => sp.GetRequiredService<ResponseInterceptor>()));
                }
                else if (interceptors?.Any() == true)
                {
                    foreach (var interceptorRegistration in interceptors)
                        opts.InterceptorRegistrations.Add(interceptorRegistration);
                }

                opts.Address = new Uri(serviceUrl);
                opts.ChannelOptionsActions.Add(channelOpts =>
                {
                    channelOpts.HttpHandler = new SocketsHttpHandler
                    {
                        KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                        KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                        PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
                        EnableMultipleHttp2Connections = true
                    };
                });
            }).Services;
    }
}
