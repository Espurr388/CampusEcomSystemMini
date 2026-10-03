using MediatR;

namespace CampusEcomSystemMini.Application.Connections.SendRequest;

public record SendRequestCommand(
    Guid ReceiverId
) : IRequest<SendRequestResponse>;