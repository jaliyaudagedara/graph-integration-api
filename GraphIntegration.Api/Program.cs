using GraphIntegration.Api;
using Microsoft.Graph;
using WebApplication = Microsoft.AspNetCore.Builder.WebApplication;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IGraphClient, GraphClient>();

WebApplication app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/Users/{objectId}", async (IGraphClient graphClient, string objectId, CancellationToken cancellationToken) =>
{
    // TODO: This could be request parameters
    // Hardcoding for simplicity
    List<string> userPropertiesToSelect = new()
    {
        nameof(User.DisplayName),
        nameof(User.GivenName),
        nameof(User.Surname),
        nameof(User.CompanyName),
        nameof(User.EmployeeId)
    };

    User result = await graphClient.GetUser(objectId, userPropertiesToSelect, cancellationToken);

    return result;
});

app.Run();
