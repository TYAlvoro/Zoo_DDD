using Zoo.Domain.Common;
using Zoo.Domain.Events;
using Zoo.Domain.ValueObjects;

namespace Zoo.Domain.Entities;

public sealed class Enclosure : Entity<EnclosureId>
{
    private readonly List<AnimalId> _animals = new();
    private readonly List<DomainEvent> _events = new();

    public EnclosureType Type { get; private set; }
    public double AreaM2 { get; private set; }
    public int Capacity { get; private set; }

    public IReadOnlyCollection<AnimalId> Animals => _animals.AsReadOnly();
    public IReadOnlyCollection<DomainEvent> DomainEvents => _events.AsReadOnly();

    public Enclosure(EnclosureId id, EnclosureType type, double areaM2, int capacity)
        : base(id)
    {
        Type = type;
        AreaM2 = areaM2;
        Capacity = capacity;
    }

    public bool CanAdd() => _animals.Count < Capacity;

    public void AddAnimal(Animal animal)
    {
        if (!CanAdd()) throw new InvalidOperationException("Enclosure is full");
        _animals.Add(animal.Id);
    }

    public void RemoveAnimal(Animal animal) => _animals.Remove(animal.Id);

    public void Clean()
    {
        _events.Add(new EnclosureCleanedEvent(Id));
    }

    public void ClearEvents() => _events.Clear();
}