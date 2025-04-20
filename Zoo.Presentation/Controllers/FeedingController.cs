using Microsoft.AspNetCore.Mvc;
using Zoo.Application.Interfaces;
using Zoo.Domain.Entities;
using Zoo.Domain.ValueObjects;
using Zoo.Application.Services;

namespace Zoo.Presentation.Controllers;

[ApiController]
[Route("api/feeding")]
public sealed class FeedingController : ControllerBase
{
    private readonly IFeedingScheduleRepository _repo;
    private readonly FeedingOrganizationService _service;

    public FeedingController(
        IFeedingScheduleRepository repo,
        FeedingOrganizationService service)
    {
        _repo = repo;
        _service = service;
    }

    [HttpGet("schedule")]
    public IAsyncEnumerable<FeedingSchedule> GetAll() => _repo.GetDueAsync(TimeOnly.MinValue);

    [HttpPost("schedule")]
    public async Task<ActionResult<Guid>> AddSchedule(AddDto dto)
    {
        var fs = new FeedingSchedule(
            FeedingId.New(),
            dto.AnimalId,
            dto.Time,
            dto.Food);

        await _repo.AddAsync(fs);
        await _repo.SaveChangesAsync();

        return Ok(fs.Id.Value);
    }

    [HttpPost("feed")]
    public async Task<IActionResult> FeedNow()
    {
        await _service.FeedDueAsync(DateTime.UtcNow);
        return Ok();
    }

    public record AddDto(AnimalId AnimalId, TimeOnly Time, string Food);
}