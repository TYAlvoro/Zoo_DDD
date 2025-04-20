using Zoo.Domain.Entities;
using Zoo.Domain.ValueObjects;

namespace Zoo.Application.Interfaces;

public interface IAnimalRepository
{
    Task<Animal?> GetAsync(AnimalId id, CancellationToken ct = default);
    Task AddAsync(Animal animal, CancellationToken ct = default);
    Task RemoveAsync(Animal animal, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
    Task<IReadOnlyCollection<Animal>> AllAsync(CancellationToken ct = default);
}