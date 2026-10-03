using MediatR;

namespace CampusEcomSystemMini.Application.Posts.GetPosts;

public record GetPostsQuery : IRequest<List<GetPostsResponse>>;