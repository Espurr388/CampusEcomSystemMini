using MediatR;

namespace CampusEcomSystemMini.Application.Connections.AcceptRequest;

public record AcceptRequestCommand(
    Guid Id
) : IRequest<AcceptRequestResponse>;