namespace CampusEcomSystemMini.Application.Connections.RejectRequest;

public record RejectRequestResponse(
    Guid Id,
    string Status,
    DateTime UpdatedAt
);