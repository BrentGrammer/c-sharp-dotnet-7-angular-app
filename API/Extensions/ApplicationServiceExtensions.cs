using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

/**
* Extension method - make the class static so we don't have to instantiate
*/
public static class ApplicationServiceExtensions
{
    // use this in the args to specify the thing you are extending.
    // in Program.cs now or when you use Services you will have access to this method
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });
        // Add CORS support to prevent CORS errors
        services.AddCors();
        // add our token service for auth. Provide the interface for the service and the service class itself
        // scoped means that the service will be instantiated for the duration of the request and controller instance and disposed of after
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}
