using Azure.Identity;
using Microsoft.Graph;

namespace GraphIntegration.Api;

public class GraphClient : IGraphClient
{
    private readonly GraphServiceClient _graphServiceClient;
    public GraphClient(IConfiguration configuration)
    {
        _graphServiceClient = GetAuthenticatedGraphClient(configuration);
    }
    public async Task<User> GetUser(string objectId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(objectId);

        return await UserRequest(objectId)
            .GetAsync(cancellationToken);
    }

    public async Task<User> GetUser(string objectId, IEnumerable<string> properties, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(objectId);

        if (properties?.Any() != true)
        {
            return await GetUser(objectId, cancellationToken);
        }

        return await UserRequest(objectId)
            .Select(string.Join(",", properties))
            .GetAsync(cancellationToken);
    }

    private static GraphServiceClient GetAuthenticatedGraphClient(IConfiguration configuration)
    {
        TokenCredentialOptions tokenCredentialOptions = new()
        {
            AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
        };

        var tenantId = configuration.GetValue<string>("GraphClient:TenantId");
        var clientId = configuration.GetValue<string>("GraphClient:ClientId");
        var clientSecret = configuration.GetValue<string>("GraphClient:ClientSecret");
        var scopes = new[] { "https://graph.microsoft.com/.default" };

        ClientSecretCredential clientSecretCredential = new(tenantId, clientId, clientSecret, tokenCredentialOptions);
        return new(clientSecretCredential, scopes);
    }

    private IUserRequest UserRequest(string objectId) => _graphServiceClient.Users[objectId].Request();
}