using AzsApi.Data;
using AzsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AzsApi.Services;

public interface IAzsService : ICommonService<Azs, int>
{
    Task<List<Azs>> GetByFirmaAsync(string nazvanie);
}

public class AzsService : IAzsService
{
    private readonly AzsContext _context;

    public AzsService(AzsContext context)
    {
        _context = context;
    }

    public async Task<List<Azs>> GetAllAsync()
    {
        return await _context.AzsStancii.ToListAsync();
    }

    public async Task<Azs?> GetByIdAsync(int id)
    {
        return await _context.AzsStancii.FindAsync(id);
    }

    // Все АЗС одной фирмы
    public async Task<List<Azs>> GetByFirmaAsync(string nazvanie)
    {
        return await _context.AzsStancii.Where(x => x.NazvanieFirmy == nazvanie).ToListAsync();
    }

    public async Task<ServiceResult> CreateAsync(Azs item)
    {
        if (!await _context.Firmy.AnyAsync(x => x.NazvanieFirmy == item.NazvanieFirmy))
            return ServiceResult.BadRequest("Фирма с таким названием не найдена");

        item.KodAzs = 0;
        _context.AzsStancii.Add(item);
        await _context.SaveChangesAsync();
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> UpdateAsync(int id, Azs item)
    {
        if (id != item.KodAzs)
            return ServiceResult.BadRequest("Код в адресе и в теле запроса не совпадает");
        if (!await _context.AzsStancii.AnyAsync(x => x.KodAzs == id))
            return ServiceResult.NotFound();
        if (!await _context.Firmy.AnyAsync(x => x.NazvanieFirmy == item.NazvanieFirmy))
            return ServiceResult.BadRequest("Фирма с таким названием не найдена");

        _context.Entry(item).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var item = await _context.AzsStancii.FindAsync(id);
        if (item == null) return ServiceResult.NotFound();

        try
        {
            _context.AzsStancii.Remove(item);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return ServiceResult.Conflict("Нельзя удалить АЗС: на ней есть продажи");
        }
        return ServiceResult.Ok();
    }
}
