using AzsApi.Models;
using AzsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzsApi.Controllers;

[ApiController]
[Route("api/azs")]
public class AzsController : ServiceControllerBase
{
    private readonly IAzsService _service;

    public AzsController(IAzsService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Azs>>> GetAll()
    {
        return await _service.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Azs>> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item == null) return NotFound();
        return item;
    }

    // Все АЗС одной фирмы
    [HttpGet("by-firma/{nazvanie}")]
    public async Task<ActionResult<IEnumerable<Azs>>> GetByFirma(string nazvanie)
    {
        return await _service.GetByFirmaAsync(nazvanie);
    }

    [HttpPost]
    public async Task<ActionResult<Azs>> Create(Azs item)
    {
        var result = await _service.CreateAsync(item);
        if (!result.IsOk) return ToActionResult(result);
        return CreatedAtAction(nameof(GetById), new { id = item.KodAzs }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Azs item)
    {
        return ToActionResult(await _service.UpdateAsync(id, item));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return ToActionResult(await _service.DeleteAsync(id));
    }
}
