using FluentAssertions;
using Zoo.Domain.Entities;
using Zoo.Domain.Events;
using Zoo.Domain.ValueObjects;

namespace Zoo.Tests;

public class AnimalTests
{
    [Fact]
    public void MoveTo_ShouldChangeEnclosureAndRaiseEvent()
    {
        var from = EnclosureId.New();
        var to = EnclosureId.New();
        var a = new Animal(
            AnimalId.New(), "Cat", "Whiskers", new DateOnly(2020,1,1),
            Gender.Female, "Fish", AnimalStatus.Healthy, from);

        a.MoveTo(to);

        a.EnclosureId.Should().Be(to);
        a.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<AnimalMovedEvent>();
    }

    [Fact]
    public void Heal_ShouldSetStatusHealthy()
    {
        var a = new Animal(
            AnimalId.New(), "Dog", "Buddy", new DateOnly(2019,5,5),
            Gender.Male, "Bone", AnimalStatus.Sick, EnclosureId.New());

        a.Heal();

        a.Status.Should().Be(AnimalStatus.Healthy);
    }
}