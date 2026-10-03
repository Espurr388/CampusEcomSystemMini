namespace CampusEcomSystemMini.Application.Posts.GetPosts;

public record GetPostsResponse(
    Guid Id,
    Guid UserId,
    string Title,
    string Content,
    string? Type,
    DateTime CreatedAt,
    DateTime UpdatedAt
);