using AzsApi.Models;
using AzsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzsApi.Controllers;

// Связь «фирма — топливо» (многие ко многим)
[ApiController]
[Route("api/firmy-toplivo")]
public class FirmyToplivoController : ServiceControllerBase
{
    private readonly IFirmyToplivoService _service;

    public FirmyToplivoController(IFirmyToplivoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FirmaToplivo>>> GetAll()
    {
        return await _service.GetAllAsync();
    }

    // Какое топливо поставляет фирма
    [HttpGet("by-firma/{nazvanie}")]
    public async Task<ActionResult<IEnumerable<FirmaToplivo>>> GetByFirma(string nazvanie)
    {
        return await _service.GetByFirmaAsync(nazvanie);
    }

    [HttpPost]
    public async Task<ActionResult<FirmaToplivo>> Create(FirmaToplivo item)
    {
        var result = await _service.CreateAsync(item);
        if (!result.IsOk) return ToActionResult(result);
        return Ok(item);
    }

    [HttpDelete("{nazvanie}/{kodTopliva}")]
    public async Task<IActionResult> Delete(string nazvanie, int kodTopliva)
    {
        return ToActionResult(await _service.DeleteAsync(nazvanie, kodTopliva));
    }
}
