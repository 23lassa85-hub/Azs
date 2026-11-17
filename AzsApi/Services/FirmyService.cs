using AzsApi.Data;
using AzsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AzsApi.Services;

public interface IFirmyService : ICommonService<Firma, string>
{
}

public class FirmyService : IFirmyService
{
    private readonly AzsContext _context;

    public FirmyService(AzsContext context)
    {
        _context = context;
    }

    public async Task<List<Firma>> GetAllAsync()
    {
        return await _context.Firmy.ToListAsync();
    }

    public async Task<Firma?> GetByIdAsync(string id)
    {
        return await _context.Firmy.FindAsync(id);
    }

    public async Task<ServiceResult> CreateAsync(Firma item)
    {
        if (await _context.Firmy.AnyAsync(x => x.NazvanieFirmy == item.NazvanieFirmy))
            return ServiceResult.Conflict("Фирма с таким названием уже есть");

        _context.Firmy.Add(item);
        await _context.SaveChangesAsync();
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> UpdateAsync(string id, Firma item)
    {
        if (id != item.NazvanieFirmy)
            return ServiceResult.BadRequest("Название в адресе и в теле запроса не совпадает");
        if (!await _context.Firmy.AnyAsync(x => x.NazvanieFirmy == id))
            return ServiceResult.NotFound();

        _context.Entry(item).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> DeleteAsync(string id)
    {
        var item = await _context.Firmy.FindAsync(id);
        if (item == null) return ServiceResult.NotFound();

        try
        {
            _context.Firmy.Remove(item);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return ServiceResult.Conflict("Нельзя удалить фирму: у неё есть АЗС или поставляемое топливо");
        }
        return ServiceResult.Ok();
    }
}
