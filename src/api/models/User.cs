public enum UserRole
{
    User,
    Manager,
    Admin // SuperAdmin
}

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }
    public bool IsBanned { get; set; }
    public bool IsFrozen { get; set; }
    public bool IsDeleted { get; set; }
    public string PasswordHash { get; set; }

    public ICollection<ManagerApplication> ManagerApplications { get; set; }
}
