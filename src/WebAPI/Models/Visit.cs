namespace GraphVisitor.WebApi.Models;

public class Visit
{
    public int Id { get; set; }

    public Staff StaffMember { get; set; }
    public int StaffMemberId { get; set; }

    public Visitor Visitor { get; set; }
    public int VisitorId { get; set; }

    public DateTime SignedIn { get; set; }

    public DateTime? SignedOut { get; set; }
}
