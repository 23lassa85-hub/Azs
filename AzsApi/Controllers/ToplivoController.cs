using AzsApi.Models;
using AzsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzsApi.Controllers;

[ApiController]
[Route("api/toplivo")]
public class ToplivoController : ServiceControllerBase
{
    private readonly IToplivoService _service;

    public ToplivoController(IToplivoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Toplivo>>> GetAll()
    {
        return await _service.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Toplivo>> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item == null) return NotFound();
        return item;
    }

    [HttpPost]
    public async Task<ActionResult<Toplivo>> Create(Toplivo item)
    {
        var result = await _service.CreateAsync(item);
        if (!result.IsOk) return ToActionResult(result);
        return CreatedAtAction(nameof(GetById), new { id = item.KodTopliva }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Toplivo item)
    {
        return ToActionResult(await _service.UpdateAsync(id, item));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return ToActionResult(await _service.DeleteAsync(id));
    }
}
