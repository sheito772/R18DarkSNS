public enum ManagerStatus
{
    Pending,
    Approved,
    Removed,
    Rejected
}
public class ManagerApplication
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public ManagerStatus Status { get; set; }
    public DateTime AppliedAt { get; set; }
    public DateTime? ActionedAt { get; set; }
    public string ActionBy { get; set; }
    public string Remarks { get; set; }
}
