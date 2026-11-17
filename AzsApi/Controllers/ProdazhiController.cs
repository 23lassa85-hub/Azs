using AzsApi.Models;
using AzsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzsApi.Controllers;

[ApiController]
[Route("api/prodazhi")]
public class ProdazhiController : ServiceControllerBase
{
    private readonly IProdazhiService _service;

    public ProdazhiController(IProdazhiService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Prodazha>>> GetAll()
    {
        return await _service.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Prodazha>> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item == null) return NotFound();
        return item;
    }

    // Все продажи одной АЗС
    [HttpGet("by-azs/{kodAzs}")]
    public async Task<ActionResult<IEnumerable<Prodazha>>> GetByAzs(int kodAzs)
    {
        return await _service.GetByAzsAsync(kodAzs);
    }

    // Все покупки одного клиента
    [HttpGet("by-klient/{kartSchet}")]
    public async Task<ActionResult<IEnumerable<Prodazha>>> GetByKlient(string kartSchet)
    {
        return await _service.GetByKlientAsync(kartSchet);
    }

    [HttpPost]
    public async Task<ActionResult<Prodazha>> Create(Prodazha item)
    {
        var result = await _service.CreateAsync(item);
        if (!result.IsOk) return ToActionResult(result);
        return CreatedAtAction(nameof(GetById), new { id = item.KodProdazhi }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Prodazha item)
    {
        return ToActionResult(await _service.UpdateAsync(id, item));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return ToActionResult(await _service.DeleteAsync(id));
    }
}
