using AzsApi.Data;
using AzsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AzsApi.Services;

public interface IToplivoService : ICommonService<Toplivo, int>
{
}

public class ToplivoService : IToplivoService
{
    private readonly AzsContext _context;

    public ToplivoService(AzsContext context)
    {
        _context = context;
    }

    public async Task<List<Toplivo>> GetAllAsync()
    {
        return await _context.Topliva.ToListAsync();
    }

    public async Task<Toplivo?> GetByIdAsync(int id)
    {
        return await _context.Topliva.FindAsync(id);
    }

    public async Task<ServiceResult> CreateAsync(Toplivo item)
    {
        item.KodTopliva = 0;
        _context.Topliva.Add(item);
        await _context.SaveChangesAsync();
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> UpdateAsync(int id, Toplivo item)
    {
        if (id != item.KodTopliva)
            return ServiceResult.BadRequest("Код в адресе и в теле запроса не совпадает");
        if (!await _context.Topliva.AnyAsync(x => x.KodTopliva == id))
            return ServiceResult.NotFound();

        _context.Entry(item).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var item = await _context.Topliva.FindAsync(id);
        if (item == null) return ServiceResult.NotFound();

        try
        {
            _context.Topliva.Remove(item);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return ServiceResult.Conflict("Нельзя удалить топливо: оно есть в продажах или у фирм-поставщиков");
        }
        return ServiceResult.Ok();
    }
}
