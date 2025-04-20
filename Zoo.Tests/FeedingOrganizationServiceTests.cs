using FluentAssertions;
using Moq;
using Zoo.Application.Services;
using Zoo.Application.Interfaces;
using Zoo.Domain.Entities;
using Zoo.Domain.ValueObjects;

namespace Zoo.Tests;

public class FeedingOrganizationServiceTests
{
    [Fact]
    public async Task FeedDueAsync_ShouldFeedAllDueAndMarkDone()
    {
        var now = new DateTime(2025,4,20,9,0,0);
        var fs = new FeedingSchedule(
            FeedingId.New(), AnimalId.New(), new TimeOnly(8,0), "Apples");
        var animal = new Animal(
            fs.AnimalId, "Elephant", "Ella", new DateOnly(2018,2,2),
            Gender.Female, "Apples", AnimalStatus.Healthy, EnclosureId.New());

        var fsRepo = new Mock<IFeedingScheduleRepository>();
        fsRepo.Setup(x => x.GetDueAsync(TimeOnly.FromDateTime(now), default))
            .Returns(new[] { fs }.ToAsyncEnumerable());
        var animalRepo = new Mock<IAnimalRepository>();
        animalRepo.Setup(x => x.GetAsync(fs.AnimalId, default)).ReturnsAsync(animal);

        var svc = new FeedingOrganizationService(fsRepo.Object, animalRepo.Object);

        await svc.FeedDueAsync(now);

        fs.DoneToday.Should().BeTrue();
    }
}