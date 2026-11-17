using AzsApi.Data;
using AzsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AzsApi.Services;

// Связь «фирма — топливо» (многие ко многим). Ключ составной, поэтому свой интерфейс.
public interface IFirmyToplivoService
{
    Task<List<FirmaToplivo>> GetAllAsync();
    Task<List<FirmaToplivo>> GetByFirmaAsync(string nazvanie);
    Task<ServiceResult> CreateAsync(FirmaToplivo item);
    Task<ServiceResult> DeleteAsync(string nazvanie, int kodTopliva);
}

public class FirmyToplivoService : IFirmyToplivoService
{
    private readonly AzsContext _context;

    public FirmyToplivoService(AzsContext context)
    {
        _context = context;
    }

    public async Task<List<FirmaToplivo>> GetAllAsync()
    {
        return await _context.FirmyToplivo.ToListAsync();
    }

    // Какое топливо поставляет фирма
    public async Task<List<FirmaToplivo>> GetByFirmaAsync(string nazvanie)
    {
        return await _context.FirmyToplivo.Where(x => x.NazvanieFirmy == nazvanie).ToListAsync();
    }

    public async Task<ServiceResult> CreateAsync(FirmaToplivo item)
    {
        if (!await _context.Firmy.AnyAsync(x => x.NazvanieFirmy == item.NazvanieFirmy))
            return ServiceResult.BadRequest("Фирма с таким названием не найдена");
        if (!await _context.Topliva.AnyAsync(x => x.KodTopliva == item.KodTopliva))
            return ServiceResult.BadRequest("Топливо с таким кодом не найдено");
        if (await _context.FirmyToplivo.AnyAsync(x => x.NazvanieFirmy == item.NazvanieFirmy && x.KodTopliva == item.KodTopliva))
            return ServiceResult.Conflict("Такая связь уже есть");

        _context.FirmyToplivo.Add(item);
        await _context.SaveChangesAsync();
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> DeleteAsync(string nazvanie, int kodTopliva)
    {
        var item = await _context.FirmyToplivo.FindAsync(nazvanie, kodTopliva);
        if (item == null) return ServiceResult.NotFound();

        // Нельзя убрать топливо у фирмы, если на её АЗС уже продавалось это топливо
        bool hasSales = await _context.Prodazhi
            .AnyAsync(p => p.KodTopliva == kodTopliva && p.Azs!.NazvanieFirmy == nazvanie);
        if (hasSales)
            return ServiceResult.Conflict("Нельзя удалить связь: на АЗС этой фирмы уже есть продажи этого топлива");

        _context.FirmyToplivo.Remove(item);
        await _context.SaveChangesAsync();
        return ServiceResult.Ok();
    }
}
