using FluentAssertions;
using Moq;
using Zoo.Application.Services;
using Zoo.Application.Interfaces;
using Zoo.Domain.Entities;
using Zoo.Domain.ValueObjects;

namespace Zoo.Tests;

public class AnimalTransferServiceTests
{
    [Fact]
    public async Task MoveAsync_ShouldMoveAnimalBetweenEnclosures()
    {
        var animalId = AnimalId.New();
        var fromId = EnclosureId.New();
        var toId = EnclosureId.New();

        var animal = new Animal(
            animalId, "Lion", "Leo", new DateOnly(2020,1,1),
            Gender.Male, "Meat", AnimalStatus.Healthy, fromId);
        var from = new Enclosure(fromId, EnclosureType.Carnivore, 100, 2);
        var to = new Enclosure(toId, EnclosureType.Carnivore, 100, 2);

        var animalRepo = new Mock<IAnimalRepository>();
        var enclRepo = new Mock<IEnclosureRepository>();

        animalRepo.Setup(x => x.GetAsync(animalId, default)).ReturnsAsync(animal);
        enclRepo.Setup(x => x.GetAsync(fromId, default)).ReturnsAsync(from);
        enclRepo.Setup(x => x.GetAsync(toId, default)).ReturnsAsync(to);

        var svc = new AnimalTransferService(animalRepo.Object, enclRepo.Object);

        await svc.MoveAsync(animalId, toId);

        animal.EnclosureId.Should().Be(toId);
        from.Animals.Should().NotContain(animalId);
        to.Animals.Should().Contain(animalId);
    }
}