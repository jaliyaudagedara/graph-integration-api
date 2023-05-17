using Microsoft.Graph;

namespace GraphIntegration.Api;

public interface IGraphClient
{
    Task<User> GetUser(string objectId, CancellationToken cancellationToken = default);

    Task<User> GetUser(string objectId, IEnumerable<string> properties, CancellationToken cancellationToken = default);
}