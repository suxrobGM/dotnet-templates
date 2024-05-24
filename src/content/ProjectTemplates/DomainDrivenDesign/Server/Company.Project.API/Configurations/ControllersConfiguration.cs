using System.Text;
using Company.Project.Shared.Cqrs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Company.Project.API.Configurations;

public static class ControllersConfiguration
{
    public static void ConfigureControllers(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                    new BadRequestObjectResult(Result.Failure(GetModelStateErrors(context.ModelState)));
            });
    }
    
    private static string GetModelStateErrors(ModelStateDictionary modelState)
    {
        var errors = new StringBuilder();
        foreach (var error in modelState.Values.SelectMany(modelStateValue => modelStateValue.Errors))
        {
            errors.Append($"{error.ErrorMessage} ");
        }

        return errors.ToString();
    }
}
