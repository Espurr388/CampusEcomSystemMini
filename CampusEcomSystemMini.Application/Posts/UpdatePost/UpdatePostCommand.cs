using System.ComponentModel.DataAnnotations;

using MediatR;

namespace CampusEcomSystemMini.Application.Posts.UpdatePost;

public record UpdatePostCommand(
    [Required] string Title,
    [Required] string Content,
    string? Type
) : IRequest<UpdatePostResponse?>
{
    // Id lấy từ route, không nhận từ body.
    public Guid Id { get; init; }
}