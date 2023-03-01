namespace GraphVisitor.WebApi.Models;

public class Staff
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Department { get; set; }

    public string GraphId { get; set; }

    public IEnumerable<Visit> Visits { get; set; }
}
