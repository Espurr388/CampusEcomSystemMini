using System.ComponentModel.DataAnnotations;

using MediatR;

namespace CampusEcomSystemMini.Application.Posts.CreatePost;

public record CreatePostCommand(
    [Required] string Title,
    [Required] string Content,
    string? Type
) : IRequest<CreatePostResponse>;