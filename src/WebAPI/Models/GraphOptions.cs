namespace GraphVisitor.WebApi.Models;

public class GraphOptions
{
    public string[] Scopes { get; set; }
    public string TenantId { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string TeamsSenderId { get; set; }
    public string TeamsSenderUsername { get; set; }
    public string TeamsSenderPassword { get; set; }
}
