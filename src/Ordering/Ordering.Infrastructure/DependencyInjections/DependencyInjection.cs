using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure.DependencyInjections;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, 
        Action<IInfrastructureOptions> builder)
    {
        var options = new InfrastructureOptions(services);
        builder(options);
            
        return services;
    }
}