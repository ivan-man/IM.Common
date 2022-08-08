using IM.Common.MediatR.Behaviours;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace IM.Common.MediatR;

public static class DependencyInjection
{
    public static IServiceCollection AddImMediatr(this IServiceCollection services, Type lookupType)
    {
        services.AddMediatR(lookupType ?? throw new ArgumentNullException(nameof(lookupType)));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(FluentValidationBehaviour<,>));

        return services;
    }
}
