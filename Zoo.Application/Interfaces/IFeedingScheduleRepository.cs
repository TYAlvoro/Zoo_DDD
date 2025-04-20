using Zoo.Domain.Entities;
using Zoo.Domain.ValueObjects;

namespace Zoo.Application.Interfaces;

public interface IFeedingScheduleRepository
{
    Task<FeedingSchedule?> GetAsync(FeedingId id, CancellationToken ct = default);
    IAsyncEnumerable<FeedingSchedule> GetDueAsync(TimeOnly now, CancellationToken ct = default);
    Task AddAsync(FeedingSchedule schedule, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}