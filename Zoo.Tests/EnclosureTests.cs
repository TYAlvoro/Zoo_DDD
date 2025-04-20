using FluentAssertions;
using Zoo.Domain.Entities;
using Zoo.Domain.ValueObjects;

namespace Zoo.Tests;

public class EnclosureTests
{
    [Fact]
    public void AddAnimal_ShouldAdd_WhenCapacityNotExceeded()
    {
        var e = new Enclosure(EnclosureId.New(), EnclosureType.Herbivore, 50, 1);
        var a = new Animal(
            AnimalId.New(), "Deer", "Bambi", new DateOnly(2022,3,3),
            Gender.Female, "Grass", AnimalStatus.Healthy, e.Id);

        e.AddAnimal(a);

        e.Animals.Should().Contain(a.Id);
    }

    [Fact]
    public void AddAnimal_ShouldThrow_WhenFull()
    {
        var e = new Enclosure(EnclosureId.New(), EnclosureType.Herbivore, 50, 1);
        var a1 = new Animal(
            AnimalId.New(), "Deer", "D1", new DateOnly(2022,3,3),
            Gender.Female, "Grass", AnimalStatus.Healthy, e.Id);
        var a2 = new Animal(
            AnimalId.New(), "Deer", "D2", new DateOnly(2022,3,3),
            Gender.Female, "Grass", AnimalStatus.Healthy, e.Id);

        e.AddAnimal(a1);
        Action act = () => e.AddAnimal(a2);

        act.Should().Throw<InvalidOperationException>();
    }
}