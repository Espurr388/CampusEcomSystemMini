using CampusEcomSystemMini.Domain.Entities;

namespace CampusEcomSystemMini.Application.Interfaces;

public interface IPostRepository
{
    Task<List<Post>> GetAllAsync(
        CancellationToken cancellationToken);

    Task<List<Post>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken);

    Task<Post?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task AddAsync(
        Post post,
        CancellationToken cancellationToken);

    void UpdateAsync(
        Post post,
        CancellationToken cancellationToken);

    void Remove(
        Post post);

    Task SaveChangesAsync(
        CancellationToken cancellationToken);
}