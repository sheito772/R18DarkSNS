using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly BlobServiceClient _blobService;
    public UserController(AppDbContext db, BlobServiceClient blobService)
    {
        _db = db;
        _blobService = blobService;
    }

    [HttpDelete("post/{postId}")]
    public async Task<IActionResult> DeleteOwnPost(int postId)
    {
        // ユーザー認証済み想定
        var userId = int.Parse(User.FindFirst("id").Value);
        var post = await _db.Posts.FindAsync(postId);
        if (post == null || post.UserId != userId) return NotFound();
        // Azure Blob削除
        var blobContainer = _blobService.GetBlobContainerClient("media");
        var blobName = post.Url.Split("/").Last();
        await blobContainer.DeleteBlobIfExistsAsync(blobName);
        _db.Posts.Remove(post);
        await _db.SaveChangesAsync();
        return Ok();
    }
}
