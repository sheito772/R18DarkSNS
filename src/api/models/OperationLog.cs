public class OperationLog
{
    public int Id { get; set; }
    public string EntityType { get; set; }
    public int EntityId { get; set; }
    public string Operation { get; set; }
    public string OperatedBy { get; set; }
    public DateTime OperatedAt { get; set; }
    public string Message { get; set; }
}
