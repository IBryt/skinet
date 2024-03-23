using API.Errors;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Extensions;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = ConfigureInvalidModelStateResponseFactory;
        });

        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder => builder.WithOrigins("https://localhost:4200")
                                  .AllowAnyHeader()
                                  .AllowAnyMethod());
        });

        return services;
    }

    private static IActionResult ConfigureInvalidModelStateResponseFactory(ActionContext actionContext)
    {
        var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToArray();

        var errorResponse = new ApiValidationErrorResponse
        {
            Errors = errors,
        };

        return new BadRequestObjectResult(errorResponse);
    }
}
