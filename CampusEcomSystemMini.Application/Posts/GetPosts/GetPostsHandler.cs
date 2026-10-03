using CampusEcomSystemMini.Application.Interfaces;
using MediatR;

namespace CampusEcomSystemMini.Application.Posts.GetPosts;

public class GetPostsHandler
    : IRequestHandler<GetPostsQuery, List<GetPostsResponse>>
{
    private readonly IPostRepository _postRepository;

    public GetPostsHandler(
        IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<List<GetPostsResponse>> Handle(
        GetPostsQuery request,
        CancellationToken cancellationToken)
    {
        var posts =
            await _postRepository.GetAllAsync(
                cancellationToken);

        return posts
            .Select(post => new GetPostsResponse(
                post.Id,
                post.UserId,
                post.Title,
                post.Content,
                post.Type,
                post.CreatedAt,
                post.UpdatedAt))
            .ToList();
    }
}