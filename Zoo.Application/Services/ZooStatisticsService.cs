using Zoo.Application.Interfaces;
using Zoo.Domain.ValueObjects;

namespace Zoo.Application.Services;

public sealed class ZooStatisticsService
{
    private readonly IAnimalRepository _animalRepo;
    private readonly IEnclosureRepository _enclosureRepo;

    public ZooStatisticsService(
        IAnimalRepository animalRepo,
        IEnclosureRepository enclosureRepo)
    {
        _animalRepo = animalRepo;
        _enclosureRepo = enclosureRepo;
    }

    public async Task<ZooStatsDto> GetAsync(CancellationToken ct = default)
    {
        var animals = await _animalRepo.AllAsync(ct);
        var enclosures = await _enclosureRepo.AllAsync(ct);

        return new ZooStatsDto(
            TotalAnimals: animals.Count,
            FreeEnclosures: enclosures.Count(e => e.CanAdd()),
            HealthyAnimals: animals.Count(a => a.Status == AnimalStatus.Healthy),
            SickAnimals: animals.Count(a => a.Status == AnimalStatus.Sick));
    }
}

public sealed record ZooStatsDto(
    int TotalAnimals,
    int FreeEnclosures,
    int HealthyAnimals,
    int SickAnimals);