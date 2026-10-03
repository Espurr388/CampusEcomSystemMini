using CampusEcomSystemMini.Application.Interfaces;
using MediatR;

namespace CampusEcomSystemMini.Application.Posts.UpdatePost;

public class UpdatePostHandler
    : IRequestHandler<UpdatePostCommand, UpdatePostResponse?>
{
    private readonly IPostRepository _postRepository;
    private readonly ICurrentUserService _currentUserService;

    public UpdatePostHandler(
        IPostRepository postRepository,
        ICurrentUserService currentUserService)
    {
        _postRepository = postRepository;
        _currentUserService = currentUserService;
    }

    public async Task<UpdatePostResponse?> Handle(
        UpdatePostCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        var post =
            await _postRepository.GetByIdAsync(
                request.Id,
                cancellationToken);

        // Không tồn tại hoặc không phải của người đang đăng nhập
        // => từ chối thao tác.
        if (post is null || post.UserId != userId)
        {
            return null;
        }

        post.Title = request.Title.Trim();
        post.Content = request.Content.Trim();
        post.Type = PostInput.NormalizeType(request.Type);
        post.UpdatedAt = DateTime.UtcNow;

        _postRepository.UpdateAsync(
            post,
            cancellationToken);

        await _postRepository.SaveChangesAsync(
            cancellationToken);

        return new UpdatePostResponse(
            post.Id,
            post.UserId,
            post.Title,
            post.Content,
            post.Type,
            post.CreatedAt,
            post.UpdatedAt);
    }
}