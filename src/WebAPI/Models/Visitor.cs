namespace GraphVisitor.WebApi.Models;

public class Visitor
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public IEnumerable<Visit> Visits { get; set; }
}
