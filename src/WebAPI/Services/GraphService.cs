using Azure.Identity;
using GraphVisitor.WebApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace GraphVisitor.WebApi.Services;

public class GraphService : IGraphService
{
    private readonly GraphServiceClient _graphClient;
    private readonly ILogger<GraphService> _logger;
    private readonly GraphOptions _options;

    public GraphService(IOptions<GraphOptions> options, ILogger<GraphService> logger)
    {
        _options = options.Value;
        
        // The client credentials flow requires that you request the
        // /.default scope, and preconfigure your permissions on the
        // app registration in Azure. An administrator must grant consent
        // to those permissions beforehand.
        var scopes = _options.Scopes;

        // Multi-tenant apps can use "common",
        // single-tenant apps must use the tenant ID from the Azure portal
        var tenantId = _options.TenantId;

        // Values from app registration
        var clientId = _options.ClientId;
        var clientSecret = _options.ClientSecret;

        // using Azure.Identity;
        var clientOptions = new TokenCredentialOptions
        {
            AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
        };

        // https://learn.microsoft.com/dotnet/api/azure.identity.clientsecretcredential
        var clientSecretCredential = new ClientSecretCredential(
            tenantId, clientId, clientSecret, clientOptions);

        _graphClient = new GraphServiceClient(clientSecretCredential, scopes);
        _logger = logger;
    }

    public async Task<IEnumerable<Staff>> SearchStaff(string query)
    {
        try
        {
            var searchResults = await _graphClient.Users.GetAsync(req =>
            {
                req.QueryParameters.Search = $"\"displayName:{query}\"";
                req.QueryParameters.Select = new string[] { "givenName", "surname", "Department", "Mail", "Id" };
                req.Headers.Add("ConsistencyLevel", "eventual");
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
            _logger.LogError("Error searching Graph for users", ex);

            throw;
        }
    }

    public async Task SendNotification(string staffId, string visitorName, string visitorEmail)
    {
        
        try
        {
            var chat = new Chat
            {
                ChatType = ChatType.OneOnOne,
                Members = new List<ConversationMember>
                {
                    new ConversationMember
                    {
                        OdataType = "#microsoft.graph.aadUserConversationMember",
                        Roles = new List<string>
                        {
                            "owner",
                        },
                        AdditionalData = new Dictionary<string, object>
                        {
                            {
                                "user@odata.bind" , $"https://graph.microsoft.com/v1.0/users('{_options.TeamsSenderId}')"
                            },
                        },
                    },
                    new ConversationMember
                    {
                        OdataType = "#microsoft.graph.aadUserConversationMember",
                        Roles = new List<string>
                        {
                            "owner",
                        },
                        AdditionalData = new Dictionary<string, object>
                        {
                            {
                                "user@odata.bind" , $"https://graph.microsoft.com/v1.0/users('{staffId}')"
                            },
                        },
                    },
                },
            };

            var teamsChat = await _graphClient.Chats.PostAsync(chat);
            
            var message = new ChatMessage
            {
                Body = new ItemBody
                {
                    Content = $"You have a visitor, {visitorName} ({visitorEmail})",
                    ContentType = BodyType.Html
                }
            };

            var result = await _graphClient.Chats[teamsChat.Id].Messages.PostAsync(message);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error sending visitor notification", ex);
            throw;
        }
    }
}
