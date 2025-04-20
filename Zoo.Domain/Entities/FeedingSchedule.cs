using Zoo.Domain.Common;
using Zoo.Domain.Events;
using Zoo.Domain.ValueObjects;

namespace Zoo.Domain.Entities;

public sealed class FeedingSchedule : Entity<FeedingId>
{
    private readonly List<DomainEvent> _events = new();

    public AnimalId AnimalId { get; private set; }
    public TimeOnly Time { get; private set; }
    public string Food { get; private set; }
    public bool DoneToday { get; private set; }

    public IReadOnlyCollection<DomainEvent> DomainEvents => _events.AsReadOnly();

    public FeedingSchedule(FeedingId id, AnimalId animalId, TimeOnly time, string food)
        : base(id)
    {
        AnimalId = animalId;
        Time = time;
        Food = food;
        DoneToday = false;
    }

    public void Change(TimeOnly time, string food)
    {
        Time = time;
        Food = food;
    }

    public void MarkDone()
    {
        if (!DoneToday)
        {
            DoneToday = true;
            _events.Add(new FeedingTimeEvent(Id));
        }
    }

    public void ClearEvents() => _events.Clear();
}