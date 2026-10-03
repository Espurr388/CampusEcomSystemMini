namespace CampusEcomSystemMini.Application.Connections.GetRequests;

public record GetRequestsResponse(
    IReadOnlyList<ConnectionRequestItem> Items);

public record ConnectionRequestItem(
    Guid Id,
    Guid SenderId,
    string SenderFullName,
    string SenderEmail,
    string? SenderAvatarUrl,
    Guid ReceiverId,
    string ReceiverFullName,
    string ReceiverEmail,
    string? ReceiverAvatarUrl,
    string Status,
    DateTime CreatedAt,
    DateTime UpdatedAt
);