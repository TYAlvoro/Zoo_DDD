using Zoo.Domain.Entities;
using Zoo.Domain.ValueObjects;

namespace Zoo.Application.Interfaces;

public interface IEnclosureRepository
{
    Task<Enclosure?> GetAsync(EnclosureId id, CancellationToken ct = default);
    Task AddAsync(Enclosure enclosure, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
    Task<IReadOnlyCollection<Enclosure>> AllAsync(CancellationToken ct = default);
}