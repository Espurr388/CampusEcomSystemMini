using CampusEcomSystemMini.Domain.Entities;

namespace CampusEcomSystemMini.Application.Interfaces;

public interface IConnectionRequestRepository
{
    Task<ConnectionRequest?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<bool> ExistsPendingBetweenUsersAsync(
        Guid firstUserId,
        Guid secondUserId,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<ConnectionRequest>> GetForUserAsync(
        Guid userId,
        CancellationToken cancellationToken);

    Task AddAsync(
        ConnectionRequest connectionRequest,
        CancellationToken cancellationToken);

    void UpdateAsync(
        ConnectionRequest connectionRequest,
        CancellationToken cancellationToken);

    Task SaveChangesAsync(
        CancellationToken cancellationToken);
}