using Zoo.Domain.Common;

namespace Zoo.Application.Interfaces;

public interface IEventBus
{
    Task PublishAsync(DomainEvent @event, CancellationToken ct = default);
}