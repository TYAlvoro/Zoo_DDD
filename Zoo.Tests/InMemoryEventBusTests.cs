using FluentAssertions;
using Zoo.Domain.Common;
using Zoo.Infrastructure.EventBus;

namespace Zoo.Tests;

public class InMemoryEventBusTests
{
    [Fact]
    public async Task PublishAsync_StoresEvent()
    {
        var bus = new InMemoryEventBus();
        var ev = new TestEvent();
        await bus.PublishAsync(ev);

        bus.Events.Should().ContainSingle().Which.Should().Be(ev);
    }

    private record TestEvent() : DomainEvent;
}