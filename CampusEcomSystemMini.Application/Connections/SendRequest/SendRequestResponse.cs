namespace CampusEcomSystemMini.Application.Connections.SendRequest;

public record SendRequestResponse(
    Guid Id,
    Guid SenderId,
    Guid ReceiverId,
    string Status,
    DateTime CreatedAt
);