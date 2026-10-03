using MediatR;

namespace CampusEcomSystemMini.Application.Connections.GetRequests;

public record GetRequestsQuery : IRequest<GetRequestsResponse>;