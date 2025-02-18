using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeacherPortal.Application.Common.Interfaces;

namespace TeacherPortal.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TeacherDBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SqlConnectionString")));
        services.AddScoped<ITeacherDbContext>(provider => provider.GetRequiredService<TeacherDBContext>());        
        return services;
    }
}
