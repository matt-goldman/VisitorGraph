using Azure.Identity;
using GraphVisitor.WebApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.Graph;

namespace GraphVisitor.WebApi.Services;

public class GraphService : IGraphService
{
    private readonly GraphServiceClient _graphClient;

    public GraphService(IOptions<GraphOptions> options)
    {
        var graphOptions = options.Value;
        // The client credentials flow requires that you request the
        // /.default scope, and preconfigure your permissions on the
        // app registration in Azure. An administrator must grant consent
        // to those permissions beforehand.
        var scopes = graphOptions.Scopes;

        // Multi-tenant apps can use "common",
        // single-tenant apps must use the tenant ID from the Azure portal
        var tenantId = graphOptions.TenantId;

        // Values from app registration
        var clientId = graphOptions.ClientId;
        var clientSecret = graphOptions.ClientSecret;

        // using Azure.Identity;
        var clientOptions = new TokenCredentialOptions
        {
            AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
        };

        // https://learn.microsoft.com/dotnet/api/azure.identity.clientsecretcredential
        var clientSecretCredential = new ClientSecretCredential(
            tenantId, clientId, clientSecret, clientOptions);

        _graphClient = new GraphServiceClient(clientSecretCredential, scopes);
    }

    public IEnumerable<Staff> SearchStaff(string query)
    {
        throw new NotImplementedException();
    }

    public Task SendNotification()
    {
        throw new NotImplementedException();
    }
}
