using CampusEcomSystemMini.Application.Interfaces;
using CampusEcomSystemMini.Domain.Entities;
using MediatR;

namespace CampusEcomSystemMini.Application.Connections.GetRequests;

public class GetRequestsHandler
    : IRequestHandler<GetRequestsQuery, GetRequestsResponse>
{
    private readonly IConnectionRequestRepository _connectionRequestRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetRequestsHandler(
        IConnectionRequestRepository connectionRequestRepository,
        IUserRepository userRepository,
        ICurrentUserService currentUserService)
    {
        _connectionRequestRepository = connectionRequestRepository;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<GetRequestsResponse> Handle(
        GetRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        var connectionRequests =
            await _connectionRequestRepository.GetForUserAsync(
                userId,
                cancellationToken);

        if (connectionRequests.Count == 0)
        {
            return new GetRequestsResponse(
                Array.Empty<ConnectionRequestItem>());
        }

        var userIds = connectionRequests
            .SelectMany(x => new[] { x.SenderId, x.ReceiverId })
            .Distinct()
            .ToList();

        var users = await _userRepository.GetByIdsAsync(
            userIds,
            cancellationToken);

        var usersById = users.ToDictionary(x => x.Id);

        var items = new List<ConnectionRequestItem>(
            connectionRequests.Count);

        foreach (var connectionRequest in connectionRequests)
        {
            usersById.TryGetValue(
                connectionRequest.SenderId,
                out var sender);

            usersById.TryGetValue(
                connectionRequest.ReceiverId,
                out var receiver);

            items.Add(new ConnectionRequestItem(
                connectionRequest.Id,
                connectionRequest.SenderId,
                sender?.FullName ?? string.Empty,
                sender?.Email ?? string.Empty,
                sender?.AvatarUrl,
                connectionRequest.ReceiverId,
                receiver?.FullName ?? string.Empty,
                receiver?.Email ?? string.Empty,
                receiver?.AvatarUrl,
                connectionRequest.Status,
                connectionRequest.CreatedAt,
                connectionRequest.UpdatedAt));
        }

        return new GetRequestsResponse(items);
    }
}