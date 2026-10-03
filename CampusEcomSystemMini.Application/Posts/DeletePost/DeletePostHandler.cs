using CampusEcomSystemMini.Application.Interfaces;
using MediatR;

namespace CampusEcomSystemMini.Application.Posts.DeletePost;

public class DeletePostHandler
    : IRequestHandler<DeletePostCommand, DeletePostResponse?>
{
    private readonly IPostRepository _postRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeletePostHandler(
        IPostRepository postRepository,
        ICurrentUserService currentUserService)
    {
        _postRepository = postRepository;
        _currentUserService = currentUserService;
    }

    public async Task<DeletePostResponse?> Handle(
        DeletePostCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        var post =
            await _postRepository.GetByIdAsync(
                request.Id,
                cancellationToken);

        // Chỉ chủ sở hữu mới được xóa bài đăng của mình.
        if (post is null || post.UserId != userId)
        {
            return null;
        }

        _postRepository.Remove(post);

        await _postRepository.SaveChangesAsync(
            cancellationToken);

        return new DeletePostResponse(
            true,
            "Post deleted successfully.");
    }
}