using Zoo.Application.Interfaces;

namespace Zoo.Application.Services;

public sealed class FeedingOrganizationService
{
    private readonly IFeedingScheduleRepository _scheduleRepo;
    private readonly IAnimalRepository _animalRepo;

    public FeedingOrganizationService(
        IFeedingScheduleRepository scheduleRepo,
        IAnimalRepository animalRepo)
    {
        _scheduleRepo = scheduleRepo;
        _animalRepo = animalRepo;
    }

    public async Task FeedDueAsync(DateTime now, CancellationToken ct = default)
    {
        await foreach (var entry in _scheduleRepo.GetDueAsync(TimeOnly.FromDateTime(now), ct))
        {
            var animal = await _animalRepo.GetAsync(entry.AnimalId, ct);
            if (animal is not null)
                animal.Feed(entry.Food);

            entry.MarkDone();
        }

        await _animalRepo.SaveChangesAsync(ct);
        await _scheduleRepo.SaveChangesAsync(ct);
    }
}