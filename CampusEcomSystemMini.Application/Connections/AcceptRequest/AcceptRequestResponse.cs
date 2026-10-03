namespace CampusEcomSystemMini.Application.Connections.AcceptRequest;

public record AcceptRequestResponse(
    Guid Id,
    string Status,
    DateTime UpdatedAt
);