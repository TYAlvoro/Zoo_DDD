using Microsoft.AspNetCore.Mvc;
using Zoo.Application.Interfaces;
using Zoo.Application.Services;
using Zoo.Domain.Entities;
using Zoo.Domain.ValueObjects;

namespace Zoo.Presentation.Controllers;

[ApiController]
[Route("api/animals")]
public sealed class AnimalsController : ControllerBase
{
    private readonly IAnimalRepository _repo;
    private readonly AnimalTransferService _transfer;

    public AnimalsController(
        IAnimalRepository repo,
        AnimalTransferService transfer)
    {
        _repo = repo;
        _transfer = transfer;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Animal>> Get(Guid id)
    {
        var animal = await _repo.GetAsync(new AnimalId(id));
        return animal is null ? NotFound() : Ok(animal);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateDto dto)
    {
        var animal = new Animal(
            AnimalId.New(),
            dto.Species,
            dto.Nickname,
            dto.BirthDate,
            dto.Gender,
            dto.FavouriteFood,
            AnimalStatus.Healthy,
            dto.EnclosureId);

        await _repo.AddAsync(animal);
        await _repo.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = animal.Id.Value }, animal.Id.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var animal = await _repo.GetAsync(new AnimalId(id));
        if (animal is null) return NotFound();
        await _repo.RemoveAsync(animal);
        await _repo.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("{id:guid}/move")]
    public async Task<IActionResult> Move(Guid id, MoveDto dto)
    {
        await _transfer.MoveAsync(new AnimalId(id), dto.ToEnclosureId);
        return Ok();
    }

    public record CreateDto(
        string Species,
        string Nickname,
        DateOnly BirthDate,
        Gender Gender,
        string FavouriteFood,
        EnclosureId EnclosureId);

    public record MoveDto(EnclosureId ToEnclosureId);
}
