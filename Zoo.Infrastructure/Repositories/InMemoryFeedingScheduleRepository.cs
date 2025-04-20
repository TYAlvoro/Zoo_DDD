using Zoo.Application.Interfaces;
using Zoo.Domain.Entities;
using Zoo.Domain.ValueObjects;

namespace Zoo.Infrastructure.Repositories;

public sealed class InMemoryFeedingScheduleRepository : IFeedingScheduleRepository
{
    private readonly Dictionary<FeedingId, FeedingSchedule> _store = new();

    public Task<FeedingSchedule?> GetAsync(FeedingId id, CancellationToken ct = default) =>
        Task.FromResult(_store.GetValueOrDefault(id));

    public IAsyncEnumerable<FeedingSchedule> GetDueAsync(TimeOnly now, CancellationToken ct = default)
    {
        var due = _store.Values.Where(f => !f.DoneToday && f.Time <= now).ToList();
        return due.ToAsyncEnumerable();
    }

    public Task AddAsync(FeedingSchedule schedule, CancellationToken ct = default)
    {
        _store[schedule.Id] = schedule;
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct = default) => Task.CompletedTask;
}