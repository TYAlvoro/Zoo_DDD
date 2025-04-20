using Zoo.Application.Interfaces;
using Zoo.Domain.Common;

namespace Zoo.Infrastructure.EventBus;

public sealed class InMemoryEventBus : IEventBus
{
    public readonly List<DomainEvent> Events = new();

    public Task PublishAsync(DomainEvent @event, CancellationToken ct = default)
    {
        Events.Add(@event);
        Console.WriteLine($"Event published: {@event.GetType().Name}");
        return Task.CompletedTask;
    }
}