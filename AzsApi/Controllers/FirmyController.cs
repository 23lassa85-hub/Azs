using AzsApi.Models;
using AzsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzsApi.Controllers;

[ApiController]
[Route("api/firmy")]
public class FirmyController : ServiceControllerBase
{
    private readonly IFirmyService _service;

    public FirmyController(IFirmyService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Firma>>> GetAll()
    {
        return await _service.GetAllAsync();
    }

    [HttpGet("{nazvanie}")]
    public async Task<ActionResult<Firma>> GetById(string nazvanie)
    {
        var item = await _service.GetByIdAsync(nazvanie);
        if (item == null) return NotFound();
        return item;
    }

    [HttpPost]
    public async Task<ActionResult<Firma>> Create(Firma item)
    {
        var result = await _service.CreateAsync(item);
        if (!result.IsOk) return ToActionResult(result);
        return CreatedAtAction(nameof(GetById), new { nazvanie = item.NazvanieFirmy }, item);
    }

    [HttpPut("{nazvanie}")]
    public async Task<IActionResult> Update(string nazvanie, Firma item)
    {
        return ToActionResult(await _service.UpdateAsync(nazvanie, item));
    }

    [HttpDelete("{nazvanie}")]
    public async Task<IActionResult> Delete(string nazvanie)
    {
        return ToActionResult(await _service.DeleteAsync(nazvanie));
    }
}
