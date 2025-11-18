using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _db;

    public AdminController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("manager/applications")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetManagerApplications()
    {
        var list = await _db.ManagerApplications.Include(x => x.User).OrderByDescending(x => x.AppliedAt).ToListAsync();
        return Ok(list);
    }

    [HttpPost("manager/approve/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ApproveManager(int id, [FromBody] string remarks)
    {
        var app = await _db.ManagerApplications.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
        if (app == null || app.Status != ManagerStatus.Pending) return NotFound();
        app.Status = ManagerStatus.Approved;
        app.ActionedAt = DateTime.Now;
        app.ActionBy = User.Identity.Name;
        app.Remarks = remarks;
        app.User.Role = UserRole.Manager;
        await _db.SaveChangesAsync();

        _db.OperationLogs.Add(new OperationLog {
            EntityType = "ManagerApplication",
            EntityId = id,
            Operation = "ApproveManager",
            OperatedBy = User.Identity.Name,
            OperatedAt = DateTime.Now,
            Message = remarks
        });
        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("manager/reject/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RejectManager(int id, [FromBody] string remarks)
    {
        var app = await _db.ManagerApplications.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
        if (app == null || app.Status != ManagerStatus.Pending) return NotFound();
        app.Status = ManagerStatus.Rejected;
        app.ActionedAt = DateTime.Now;
        app.ActionBy = User.Identity.Name;
        app.Remarks = remarks;
        await _db.SaveChangesAsync();

        _db.OperationLogs.Add(new OperationLog {
            EntityType = "ManagerApplication",
            EntityId = id,
            Operation = "RejectManager",
            OperatedBy = User.Identity.Name,
            OperatedAt = DateTime.Now,
            Message = remarks
        });
        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("manager/remove/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveManager(int userId, [FromBody] string remarks)
    {
        var user = await _db.Users.Include(u => u.ManagerApplications).FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null || user.Role != UserRole.Manager) return NotFound();
        user.Role = UserRole.User;
        user.IsDeleted = true;

        var app = new ManagerApplication {
            UserId = userId,
            Status = ManagerStatus.Removed,
            AppliedAt = DateTime.Now,
            ActionedAt = DateTime.Now,
            ActionBy = User.Identity.Name,
            Remarks = remarks
        };
        _db.ManagerApplications.Add(app);

        _db.OperationLogs.Add(new OperationLog {
            EntityType = "User",
            EntityId = userId,
            Operation = "DeleteManager",
            OperatedBy = User.Identity.Name,
            OperatedAt = DateTime.Now,
            Message = remarks
        });
        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("manager/restore/{applicationId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RestoreManager(int applicationId, [FromBody] string remarks)
    {
        var app = await _db.ManagerApplications.Include(a => a.User).FirstOrDefaultAsync(a => a.Id == applicationId);
        if (app == null || app.Status != ManagerStatus.Removed) return NotFound();
        app.Status = ManagerStatus.Approved;
        app.ActionedAt = DateTime.Now;
        app.ActionBy = User.Identity.Name;
        app.Remarks = remarks;
        app.User.Role = UserRole.Manager;
        app.User.IsDeleted = false;
        await _db.SaveChangesAsync();

        _db.OperationLogs.Add(new OperationLog {
            EntityType = "ManagerApplication",
            EntityId = applicationId,
            Operation = "RestoreManager",
            OperatedBy = User.Identity.Name,
            OperatedAt = DateTime.Now,
            Message = remarks
        });
        await _db.SaveChangesAsync();
        return Ok();
    }
}
