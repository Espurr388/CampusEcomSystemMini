using CampusEcomSystemMini.Application.Interfaces;
using CampusEcomSystemMini.Domain.Entities;
using CampusEcomSystemMini.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CampusEcomSystemMini.Infrastructure.Repositories;

public class PostRepository : IPostRepository
{
    private readonly AppDbContext _context;

    public PostRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Post>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        return await _context.Posts
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Post>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        return await _context.Posts
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Post?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.Posts
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task AddAsync(
        Post post,
        CancellationToken cancellationToken)
    {
        await _context.Posts.AddAsync(
            post,
            cancellationToken);
    }

    public void UpdateAsync(
        Post post,
        CancellationToken cancellationToken)
    {
        _context.Posts.Update(post);
    }

    public void Remove(
        Post post)
    {
        _context.Posts.Remove(post);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}