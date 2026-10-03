using CampusEcomSystemMini.Application.Interfaces;
using CampusEcomSystemMini.Domain.Entities;
using MediatR;

namespace CampusEcomSystemMini.Application.Posts.CreatePost;

public class CreatePostHandler
    : IRequestHandler<CreatePostCommand, CreatePostResponse>
{
    private readonly IPostRepository _postRepository;
    private readonly ICurrentUserService _currentUserService;

    public CreatePostHandler(
        IPostRepository postRepository,
        ICurrentUserService currentUserService)
    {
        _postRepository = postRepository;
        _currentUserService = currentUserService;
    }

    public async Task<CreatePostResponse> Handle(
        CreatePostCommand request,
        CancellationToken cancellationToken)
    {
        // Chủ sở hữu luôn lấy từ người dùng đang đăng nhập.
        var userId = _currentUserService.UserId;

        var now = DateTime.UtcNow;

        var post = new Post
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = request.Title.Trim(),
            Content = request.Content.Trim(),
            Type = PostInput.NormalizeType(request.Type),
            CreatedAt = now,
            UpdatedAt = now
        };

        await _postRepository.AddAsync(
            post,
            cancellationToken);

        await _postRepository.SaveChangesAsync(
            cancellationToken);

        return new CreatePostResponse(
            post.Id,
            post.UserId,
            post.Title,
            post.Content,
            post.Type,
            post.CreatedAt,
            post.UpdatedAt);
    }
}