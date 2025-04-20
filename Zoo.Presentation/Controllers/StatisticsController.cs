using Microsoft.AspNetCore.Mvc;
using Zoo.Application.Services;

namespace Zoo.Presentation.Controllers;

[ApiController]
[Route("api/stats")]
public sealed class StatisticsController : ControllerBase
{
    private readonly ZooStatisticsService _service;

    public StatisticsController(ZooStatisticsService service) =>
        _service = service;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var stats = await _service.GetAsync();
        return Ok(stats);
    }
}