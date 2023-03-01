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

    public async Task<IEnumerable<Staff>> SearchStaff(string query)
    {
        try
        {
            var searchResults = await _graphClient.Users.GetAsync(req =>
            {
                req.QueryParameters.Search = $"displayName:{query}";
                req.QueryParameters.Select = new string[] { "displayName", "department", "email" };
            });

            var results = new List<Staff>();

            foreach (var result in searchResults.Value)
            {
                results.Add(new Staff
                {
                    Department = result.Department,
                    FirstName = result.GivenName,
                    LastName = result.Surname,
                    Email = result.Mail,
                    GraphId = result.Id
                });
            }

            return results;
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public Task SendNotification()
    {
        throw new NotImplementedException();
    }
}
