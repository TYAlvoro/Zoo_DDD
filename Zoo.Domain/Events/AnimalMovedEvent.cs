using Zoo.Domain.Common;
using Zoo.Domain.ValueObjects;

namespace Zoo.Domain.Events;

public sealed record AnimalMovedEvent(
    AnimalId AnimalId,
    EnclosureId FromEnclosure,
    EnclosureId ToEnclosure) : DomainEvent;