using AzsApi.Models;
using AzsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzsApi.Controllers;

[ApiController]
[Route("api/klienty")]
public class KlientyController : ServiceControllerBase
{
    private readonly IKlientyService _service;

    public KlientyController(IKlientyService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Klient>>> GetAll()
    {
        return await _service.GetAllAsync();
    }

    // Клиенты, у которых нет ни одной покупки (правило 19)
    [HttpGet("neaktivnye")]
    public async Task<ActionResult<IEnumerable<Klient>>> GetNeaktivnye()
    {
        return await _service.GetNeaktivnyeAsync();
    }

    [HttpGet("{kartSchet}")]
    public async Task<ActionResult<Klient>> GetById(string kartSchet)
    {
        var item = await _service.GetByIdAsync(kartSchet);
        if (item == null) return NotFound();
        return item;
    }

    [HttpPost]
    public async Task<ActionResult<Klient>> Create(Klient item)
    {
        var result = await _service.CreateAsync(item);
        if (!result.IsOk) return ToActionResult(result);
        return CreatedAtAction(nameof(GetById), new { kartSchet = item.KartSchet }, item);
    }

    [HttpPut("{kartSchet}")]
    public async Task<IActionResult> Update(string kartSchet, Klient item)
    {
        return ToActionResult(await _service.UpdateAsync(kartSchet, item));
    }

    [HttpDelete("{kartSchet}")]
    public async Task<IActionResult> Delete(string kartSchet)
    {
        return ToActionResult(await _service.DeleteAsync(kartSchet));
    }
}
