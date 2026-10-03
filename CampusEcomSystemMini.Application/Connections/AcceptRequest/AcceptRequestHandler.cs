using CampusEcomSystemMini.Application.Exceptions;
using CampusEcomSystemMini.Application.Interfaces;
using MediatR;

namespace CampusEcomSystemMini.Application.Connections.AcceptRequest;

public class AcceptRequestHandler
    : IRequestHandler<AcceptRequestCommand, AcceptRequestResponse>
{
    private readonly IConnectionRequestRepository _connectionRequestRepository;
    private readonly ICurrentUserService _currentUserService;

    public AcceptRequestHandler(
        IConnectionRequestRepository connectionRequestRepository,
        ICurrentUserService currentUserService)
    {
        _connectionRequestRepository = connectionRequestRepository;
        _currentUserService = currentUserService;
    }

    public async Task<AcceptRequestResponse> Handle(
        AcceptRequestCommand request,
        CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        var connectionRequest =
            await _connectionRequestRepository.GetByIdAsync(
                request.Id,
                cancellationToken);

        if (connectionRequest is null)
        {
            throw new AppException(
                AppErrorType.NotFound,
                "Connection request not found.");
        }

        if (connectionRequest.ReceiverId != currentUserId)
        {
            throw new AppException(
                AppErrorType.Forbidden,
                "Only the receiver can accept this connection request.");
        }

        if (connectionRequest.Status != ConnectionRequestStatus.Pending)
        {
            throw new AppException(
                AppErrorType.Conflict,
                "This connection request has already been processed.");
        }

        connectionRequest.Status = ConnectionRequestStatus.Accepted;

        connectionRequest.UpdatedAt = DateTime.UtcNow;

        _connectionRequestRepository.UpdateAsync(
            connectionRequest,
            cancellationToken);

        await _connectionRequestRepository.SaveChangesAsync(
            cancellationToken);

        return new AcceptRequestResponse(
            connectionRequest.Id,
            connectionRequest.Status,
            connectionRequest.UpdatedAt);
    }
}