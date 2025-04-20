using FluentAssertions;
using Zoo.Domain.Entities;
using Zoo.Domain.ValueObjects;
using Zoo.Infrastructure.Repositories;

namespace Zoo.Tests;

public class InMemoryFeedingScheduleRepositoryTests
{
    private readonly InMemoryFeedingScheduleRepository _repo = new();

    [Fact]
    public async Task AddAndGet_ReturnsSameSchedule()
    {
        var id = FeedingId.New();
        var fs = new FeedingSchedule(id, AnimalId.New(), new TimeOnly(9, 0), "Apples");

        await _repo.AddAsync(fs);
        var fetched = await _repo.GetAsync(id);

        fetched.Should().NotBeNull().And.Be(fs);
    }

    [Fact]
    public async Task GetDueAsync_ReturnsOnlyNotDoneAndDue()
    {
        var now = new TimeOnly(10, 0);
        var due = new FeedingSchedule(FeedingId.New(), AnimalId.New(), new TimeOnly(9, 0), "X");
        var notDue = new FeedingSchedule(FeedingId.New(), AnimalId.New(), new TimeOnly(11, 0), "Y");
        due.MarkDone(); // отметим и очистим
        due = new FeedingSchedule(due.Id, due.AnimalId, new TimeOnly(9,0), "X");
        await _repo.AddAsync(due);
        await _repo.AddAsync(notDue);

        var list = new List<FeedingSchedule>();
        await foreach (var f in _repo.GetDueAsync(now))
            list.Add(f);

        list.Should().ContainSingle().Which.Should().Be(due);
    }
}