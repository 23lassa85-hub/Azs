using AzsApi.Data;
using AzsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AzsApi.Services;

public interface IKlientyService : ICommonService<Klient, string>
{
    Task<List<Klient>> GetNeaktivnyeAsync();
}

public class KlientyService : IKlientyService
{
    private readonly AzsContext _context;

    public KlientyService(AzsContext context)
    {
        _context = context;
    }

    public async Task<List<Klient>> GetAllAsync()
    {
        return await _context.Klienty.ToListAsync();
    }

    public async Task<Klient?> GetByIdAsync(string id)
    {
        return await _context.Klienty.FindAsync(id);
    }

    // Клиенты, у которых нет ни одной покупки (правило 19)
    public async Task<List<Klient>> GetNeaktivnyeAsync()
    {
        return await _context.Klienty.Where(k => !k.Prodazhi.Any()).ToListAsync();
    }

    public async Task<ServiceResult> CreateAsync(Klient item)
    {
        if (await _context.Klienty.AnyAsync(x => x.KartSchet == item.KartSchet))
            return ServiceResult.Conflict("Клиент с таким карт-счётом уже есть");

        _context.Klienty.Add(item);
        await _context.SaveChangesAsync();
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> UpdateAsync(string id, Klient item)
    {
        if (id != item.KartSchet)
            return ServiceResult.BadRequest("Карт-счёт в адресе и в теле запроса не совпадает");
        if (!await _context.Klienty.AnyAsync(x => x.KartSchet == id))
            return ServiceResult.NotFound();

        _context.Entry(item).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> DeleteAsync(string id)
    {
        var item = await _context.Klienty.FindAsync(id);
        if (item == null) return ServiceResult.NotFound();

        try
        {
            _context.Klienty.Remove(item);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return ServiceResult.Conflict("Нельзя удалить клиента: у него есть продажи");
        }
        return ServiceResult.Ok();
    }
}
