using FluentAssertions;
using Moq;
using Zoo.Application.Services;
using Zoo.Application.Interfaces;
using Zoo.Domain.Entities;
using Zoo.Domain.ValueObjects;

namespace Zoo.Tests;

public class ZooStatisticsServiceTests
{
    [Fact]
    public async Task GetAsync_ShouldReturnCorrectCounts()
    {
        var a1 = new Animal(AnimalId.New(), "A","N", DateOnly.MinValue, Gender.Male, "F", AnimalStatus.Healthy, EnclosureId.New());
        var a2 = new Animal(AnimalId.New(), "B","N", DateOnly.MinValue, Gender.Female, "F", AnimalStatus.Sick,    EnclosureId.New());
        var e1 = new Enclosure(EnclosureId.New(), EnclosureType.Bird, 10, 1);
        var e2 = new Enclosure(EnclosureId.New(), EnclosureType.Bird, 10, 2);
        e1.AddAnimal(a1);

        var animalRepo = new Mock<IAnimalRepository>();
        animalRepo.Setup(x => x.AllAsync(default))
            .ReturnsAsync(new[] { a1, a2 });
        var enclRepo = new Mock<IEnclosureRepository>();
        enclRepo.Setup(x => x.AllAsync(default))
            .ReturnsAsync(new[] { e1, e2 });

        var svc = new ZooStatisticsService(animalRepo.Object, enclRepo.Object);

        var stats = await svc.GetAsync();

        stats.TotalAnimals.Should().Be(2);
        stats.HealthyAnimals.Should().Be(1);
        stats.SickAnimals.Should().Be(1);
        stats.FreeEnclosures.Should().Be(1);
    }
}