using CampusEcomSystemMini.Application.Exceptions;
using CampusEcomSystemMini.Application.Interfaces;
using CampusEcomSystemMini.Domain.Entities;
using MediatR;

namespace CampusEcomSystemMini.Application.Connections.SendRequest;

public class SendRequestHandler
    : IRequestHandler<SendRequestCommand, SendRequestResponse>
{
    private readonly IConnectionRequestRepository _connectionRequestRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public SendRequestHandler(
        IConnectionRequestRepository connectionRequestRepository,
        IUserRepository userRepository,
        ICurrentUserService currentUserService)
    {
        _connectionRequestRepository = connectionRequestRepository;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<SendRequestResponse> Handle(
        SendRequestCommand request,
        CancellationToken cancellationToken)
    {
        var senderId = _currentUserService.UserId;

        if (request.ReceiverId == Guid.Empty)
        {
            throw new AppException(
                AppErrorType.Validation,
                "ReceiverId is required.");
        }

        if (request.ReceiverId == senderId)
        {
            throw new AppException(
                AppErrorType.Validation,
                "You cannot send a connection request to yourself.");
        }

        var receiver = await _userRepository.GetByIdAsync(
            request.ReceiverId,
            cancellationToken);

        if (receiver is null)
        {
            throw new AppException(
                AppErrorType.NotFound,
                "Receiver user does not exist.");
        }

        var hasPendingRequest =
            await _connectionRequestRepository.ExistsPendingBetweenUsersAsync(
                senderId,
                request.ReceiverId,
                cancellationToken);

        if (hasPendingRequest)
        {
            throw new AppException(
                AppErrorType.Conflict,
                "A pending connection request already exists between these two users.");
        }

        var now = DateTime.UtcNow;

        var connectionRequest = new ConnectionRequest
        {
            Id = Guid.NewGuid(),
            SenderId = senderId,
            ReceiverId = request.ReceiverId,
            Status = ConnectionRequestStatus.Pending,
            CreatedAt = now,
            UpdatedAt = now
        };

        await _connectionRequestRepository.AddAsync(
            connectionRequest,
            cancellationToken);

        await _connectionRequestRepository.SaveChangesAsync(
            cancellationToken);

        return new SendRequestResponse(
            connectionRequest.Id,
            connectionRequest.SenderId,
            connectionRequest.ReceiverId,
            connectionRequest.Status,
            connectionRequest.CreatedAt);
    }
}