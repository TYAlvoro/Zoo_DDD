using FluentAssertions;
using Zoo.Domain.Entities;
using Zoo.Domain.Events;
using Zoo.Domain.ValueObjects;

namespace Zoo.Tests;

public class FeedingScheduleTests
{
    [Fact]
    public void MarkDone_ShouldSetDoneAndRaiseEventOnce()
    {
        var fs = new FeedingSchedule(
            FeedingId.New(), AnimalId.New(), new TimeOnly(12, 0), "Bananas");

        fs.MarkDone();

        fs.DoneToday.Should().BeTrue();
        fs.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<FeedingTimeEvent>();
    }
}