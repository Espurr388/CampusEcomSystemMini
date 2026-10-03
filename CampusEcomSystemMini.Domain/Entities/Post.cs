namespace CampusEcomSystemMini.Domain.Entities;

// Bài đăng của sinh viên trên campus.
// Chủ sở hữu (UserId) luôn lấy từ ICurrentUserService, không nhận từ frontend.
public class Post
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    // Nhóm nội dung: Room, Group, Lost-Found, library ...
    public string? Type { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}