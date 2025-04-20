using Zoo.Domain.Common;
using Zoo.Domain.ValueObjects;

namespace Zoo.Domain.Events;

public sealed record EnclosureCleanedEvent(EnclosureId EnclosureId) : DomainEvent;