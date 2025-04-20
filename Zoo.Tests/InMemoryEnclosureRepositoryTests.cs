using FluentAssertions;
using Zoo.Domain.Entities;
using Zoo.Domain.ValueObjects;
using Zoo.Infrastructure.Repositories;

namespace Zoo.Tests;

public class InMemoryEnclosureRepositoryTests
{
    private readonly InMemoryEnclosureRepository _repo = new();

    [Fact]
    public async Task AddAndGet_ReturnsSameEnclosure()
    {
        var id = EnclosureId.New();
        var encl = new Enclosure(id, EnclosureType.Bird, 20, 5);

        await _repo.AddAsync(encl);
        var fetched = await _repo.GetAsync(id);

        fetched.Should().NotBeNull().And.Be(encl);
    }

    [Fact]
    public async Task AllAsync_ReturnsAllEnclosures()
    {
        var e1 = new Enclosure(EnclosureId.New(), EnclosureType.Herbivore, 30, 3);
        var e2 = new Enclosure(EnclosureId.New(), EnclosureType.Carnivore, 40, 4);

        await _repo.AddAsync(e1);
        await _repo.AddAsync(e2);

        var all = await _repo.AllAsync();
        all.Should().HaveCount(2).And.Contain(new[] { e1, e2 });
    }
}