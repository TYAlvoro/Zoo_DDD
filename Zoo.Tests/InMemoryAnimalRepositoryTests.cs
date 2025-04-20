using FluentAssertions;
using Zoo.Domain.Entities;
using Zoo.Domain.ValueObjects;
using Zoo.Infrastructure.Repositories;

namespace Zoo.Tests;

public class InMemoryAnimalRepositoryTests
{
    private readonly InMemoryAnimalRepository _repo = new();

    [Fact]
    public async Task AddAndGet_ReturnsSameAnimal()
    {
        var id = AnimalId.New();
        var animal = new Animal(id, "Swan", "Snowy", DateOnly.Parse("2020-01-01"),
            Gender.Female, "Fish", AnimalStatus.Healthy, EnclosureId.New());

        await _repo.AddAsync(animal);
        var fromRepo = await _repo.GetAsync(id);

        fromRepo.Should().NotBeNull().And.Be(animal);
    }

    [Fact]
    public async Task Remove_RemovesAnimal()
    {
        var id = AnimalId.New();
        var animal = new Animal(id, "Bear", "Baloo", DateOnly.Parse("2019-05-05"),
            Gender.Male, "Honey", AnimalStatus.Healthy, EnclosureId.New());

        await _repo.AddAsync(animal);
        await _repo.RemoveAsync(animal);
        var fromRepo = await _repo.GetAsync(id);

        fromRepo.Should().BeNull();
    }

    [Fact]
    public async Task AllAsync_ReturnsAllAnimals()
    {
        var a1 = new Animal(AnimalId.New(), "A", "A", DateOnly.MinValue, Gender.Male, "F", AnimalStatus.Healthy, EnclosureId.New());
        var a2 = new Animal(AnimalId.New(), "B", "B", DateOnly.MinValue, Gender.Male, "F", AnimalStatus.Healthy, EnclosureId.New());

        await _repo.AddAsync(a1);
        await _repo.AddAsync(a2);

        var all = await _repo.AllAsync();
        all.Should().HaveCount(2).And.Contain(new[] { a1, a2 });
    }
}