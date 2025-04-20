using Microsoft.AspNetCore.Mvc;
using Zoo.Application.Interfaces;
using Zoo.Domain.Entities;
using Zoo.Domain.ValueObjects;

namespace Zoo.Presentation.Controllers;

[ApiController]
[Route("api/enclosures")]
public sealed class EnclosuresController : ControllerBase
{
    private readonly IEnclosureRepository _repo;

    public EnclosuresController(IEnclosureRepository repo) => _repo = repo;

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Enclosure>> Get(Guid id)
    {
        var encl = await _repo.GetAsync(new EnclosureId(id));
        return encl is null ? NotFound() : Ok(encl);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateDto dto)
    {
        var encl = new Enclosure(
            EnclosureId.New(),
            dto.Type,
            dto.AreaM2,
            dto.Capacity);

        await _repo.AddAsync(encl);
        await _repo.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = encl.Id.Value }, encl.Id.Value);
    }

    public record CreateDto(EnclosureType Type, double AreaM2, int Capacity);
}