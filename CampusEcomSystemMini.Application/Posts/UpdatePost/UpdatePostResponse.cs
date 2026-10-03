namespace CampusEcomSystemMini.Application.Posts.UpdatePost;

public record UpdatePostResponse(
    Guid Id,
    Guid UserId,
    string Title,
    string Content,
    string? Type,
    DateTime CreatedAt,
    DateTime UpdatedAt
);