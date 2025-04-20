using Zoo.Domain.Common;
using Zoo.Domain.Events;
using Zoo.Domain.ValueObjects;

namespace Zoo.Domain.Entities;

public sealed class Animal : Entity<AnimalId>
{
    private readonly List<DomainEvent> _events = new();

    public string Species { get; private set; }
    public string Nickname { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public Gender Gender { get; private set; }
    public string FavouriteFood { get; private set; }
    public AnimalStatus Status { get; private set; }
    public EnclosureId EnclosureId { get; private set; }

    public IReadOnlyCollection<DomainEvent> DomainEvents => _events.AsReadOnly();

    public Animal(
        AnimalId id,
        string species,
        string nickname,
        DateOnly birthDate,
        Gender gender,
        string favouriteFood,
        AnimalStatus status,
        EnclosureId enclosureId) : base(id)
    {
        Species = species;
        Nickname = nickname;
        BirthDate = birthDate;
        Gender = gender;
        FavouriteFood = favouriteFood;
        Status = status;
        EnclosureId = enclosureId;
    }

    public void Feed(string food)
    {
        if (food != FavouriteFood)
        {
        }
    }

    public void Heal() => Status = AnimalStatus.Healthy;

    public void MoveTo(EnclosureId to)
    {
        var from = EnclosureId;
        EnclosureId = to;
        _events.Add(new AnimalMovedEvent(Id, from, to));
    }

    public void ClearEvents() => _events.Clear();
}