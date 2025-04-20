using Zoo.Application.Interfaces;
using Zoo.Domain.Entities;
using Zoo.Domain.ValueObjects;

namespace Zoo.Infrastructure.Repositories;

public sealed class InMemoryAnimalRepository : IAnimalRepository
{
    private readonly Dictionary<AnimalId, Animal> _store = new();

    public Task<Animal?> GetAsync(AnimalId id, CancellationToken ct = default) =>
        Task.FromResult(_store.GetValueOrDefault(id));

    public Task AddAsync(Animal animal, CancellationToken ct = default)
    {
        _store[animal.Id] = animal;
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Animal animal, CancellationToken ct = default)
    {
        _store.Remove(animal.Id);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct = default) => Task.CompletedTask;

    public Task<IReadOnlyCollection<Animal>> AllAsync(CancellationToken ct = default) =>
        Task.FromResult((IReadOnlyCollection<Animal>)_store.Values.ToList());
}