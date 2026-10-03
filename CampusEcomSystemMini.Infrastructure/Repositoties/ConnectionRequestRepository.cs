using CampusEcomSystemMini.Application.Connections;
using CampusEcomSystemMini.Application.Interfaces;
using CampusEcomSystemMini.Domain.Entities;
using CampusEcomSystemMini.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CampusEcomSystemMini.Infrastructure.Repositories;

public class ConnectionRequestRepository : IConnectionRequestRepository
{
    private readonly AppDbContext _context;

    public ConnectionRequestRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ConnectionRequest?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.ConnectionRequests
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<bool> ExistsPendingBetweenUsersAsync(
        Guid firstUserId,
        Guid secondUserId,
        CancellationToken cancellationToken)
    {
        return await _context.ConnectionRequests
            .AnyAsync(
                x => x.Status == ConnectionRequestStatus.Pending
                    && ((x.SenderId == firstUserId
                            && x.ReceiverId == secondUserId)
                        || (x.SenderId == secondUserId
                            && x.ReceiverId == firstUserId)),
                cancellationToken);
    }

    public async Task<IReadOnlyList<ConnectionRequest>> GetForUserAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        return await _context.ConnectionRequests
            .Where(
                x => x.SenderId == userId
                    || x.ReceiverId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(
                cancellationToken);
    }

    public async Task AddAsync(
        ConnectionRequest connectionRequest,
        CancellationToken cancellationToken)
    {
        await _context.ConnectionRequests.AddAsync(
            connectionRequest,
            cancellationToken);
    }

    public void UpdateAsync(
        ConnectionRequest connectionRequest,
        CancellationToken cancellationToken)
    {
        _context.ConnectionRequests.Update(connectionRequest);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(
            cancellationToken);
    }
}