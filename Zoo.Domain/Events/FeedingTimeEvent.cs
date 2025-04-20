using Zoo.Domain.Common;
using Zoo.Domain.ValueObjects;

namespace Zoo.Domain.Events;

public sealed record FeedingTimeEvent(FeedingId FeedingId) : DomainEvent;