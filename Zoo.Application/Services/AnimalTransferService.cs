using Zoo.Application.Interfaces;
using Zoo.Domain.Entities;
using Zoo.Domain.ValueObjects;

namespace Zoo.Application.Services;

public sealed class AnimalTransferService
{
    private readonly IAnimalRepository _animals;
    private readonly IEnclosureRepository _enclosures;

    public AnimalTransferService(
        IAnimalRepository animals,
        IEnclosureRepository enclosures)
    {
        _animals = animals;
        _enclosures = enclosures;
    }

    public async Task MoveAsync(
        AnimalId animalId,
        EnclosureId to,
        CancellationToken ct = default)
    {
        var animal = await _animals.GetAsync(animalId, ct)
                     ?? throw new InvalidOperationException("Animal not found");
        var fromEncl = await _enclosures.GetAsync(animal.EnclosureId, ct)
                       ?? throw new InvalidOperationException("Current enclosure missing");
        var toEncl = await _enclosures.GetAsync(to, ct)
                     ?? throw new InvalidOperationException("Target enclosure missing");

        if (!toEncl.CanAdd())
            throw new InvalidOperationException("Target enclosure is full");

        fromEncl.RemoveAnimal(animal);
        toEncl.AddAnimal(animal);
        animal.MoveTo(to);

        await _enclosures.SaveChangesAsync(ct);
        await _animals.SaveChangesAsync(ct);
    }
}