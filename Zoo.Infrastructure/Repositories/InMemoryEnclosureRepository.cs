using Zoo.Application.Interfaces;
using Zoo.Domain.Entities;
using Zoo.Domain.ValueObjects;

namespace Zoo.Infrastructure.Repositories;

public sealed class InMemoryEnclosureRepository : IEnclosureRepository
{
    private readonly Dictionary<EnclosureId, Enclosure> _store = new();

    public Task<Enclosure?> GetAsync(EnclosureId id, CancellationToken ct = default) =>
        Task.FromResult(_store.GetValueOrDefault(id));

    public Task AddAsync(Enclosure enclosure, CancellationToken ct = default)
    {
        _store[enclosure.Id] = enclosure;
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct = default) => Task.CompletedTask;

    public Task<IReadOnlyCollection<Enclosure>> AllAsync(CancellationToken ct = default) =>
        Task.FromResult((IReadOnlyCollection<Enclosure>)_store.Values.ToList());
}