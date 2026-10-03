namespace CampusEcomSystemMini.Application.Posts.CreatePost;

public record CreatePostResponse(
    Guid Id,
    Guid UserId,
    string Title,
    string Content,
    string? Type,
    DateTime CreatedAt,
    DateTime UpdatedAt
);