﻿using Azure.Identity;
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
    private readonly GraphServiceClient _teamsSenderClient;

    public GraphService(IOptions<GraphOptions> options, ILogger<GraphService> logger)
    {
        _options = options.Value;

        // Create Graph client for looking up staff and other data
        var clientOptions = new TokenCredentialOptions
        {
            AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
        };
        
        var clientSecretCredential = new ClientSecretCredential(
            _options.TenantId, _options.ClientId, _options.ClientSecret, clientOptions);

        _graphClient = new GraphServiceClient(clientSecretCredential, _options.Scopes);


        // Create Graph client for sending messages to Teams using username and password
        var teamsSenderCredential = new UsernamePasswordCredential(_options.TeamsSenderUsername, _options.TeamsSenderPassword, _options.TenantId, _options.ClientId, clientOptions);

        _teamsSenderClient = new GraphServiceClient(teamsSenderCredential, _options.Scopes);

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
                }
            };

            var teamsChat = await _graphClient.Chats.PostAsync(chat);

            var message = new ChatMessage
            {
                Body = new ItemBody
                {
                    Content = $"Hello, {visitorName} ({visitorEmail}) is waiting for you in the lobby.",
                    ContentType = BodyType.Html,
                },
                Importance = ChatMessageImportance.Normal,
                MessageType = ChatMessageType.Message,
                Locale = "en/AU"
            };

            await _teamsSenderClient.Chats[teamsChat.Id].Messages.PostAsync(message);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error sending visitor notification", ex);
            throw;
        }
    }
}
