using AzsApi.Data;
using AzsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AzsApi.Services;

public interface IProdazhiService : ICommonService<Prodazha, int>
{
    Task<List<Prodazha>> GetByAzsAsync(int kodAzs);
    Task<List<Prodazha>> GetByKlientAsync(string kartSchet);
}

public class ProdazhiService : IProdazhiService
{
    private readonly AzsContext _context;

    public ProdazhiService(AzsContext context)
    {
        _context = context;
    }

    public async Task<List<Prodazha>> GetAllAsync()
    {
        return await _context.Prodazhi.OrderBy(x => x.DataProdazhi).ToListAsync();
    }

    public async Task<Prodazha?> GetByIdAsync(int id)
    {
        return await _context.Prodazhi.FindAsync(id);
    }

    // Все продажи одной АЗС
    public async Task<List<Prodazha>> GetByAzsAsync(int kodAzs)
    {
        return await _context.Prodazhi.Where(x => x.KodAzs == kodAzs).ToListAsync();
    }

    // Все покупки одного клиента
    public async Task<List<Prodazha>> GetByKlientAsync(string kartSchet)
    {
        return await _context.Prodazhi.Where(x => x.KartSchet == kartSchet).ToListAsync();
    }

    public async Task<ServiceResult> CreateAsync(Prodazha item)
    {
        var error = await Validate(item);
        if (error != null) return ServiceResult.BadRequest(error);

        item.KodProdazhi = 0;
        _context.Prodazhi.Add(item);
        await _context.SaveChangesAsync();
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> UpdateAsync(int id, Prodazha item)
    {
        if (id != item.KodProdazhi)
            return ServiceResult.BadRequest("Код в адресе и в теле запроса не совпадает");
        if (!await _context.Prodazhi.AnyAsync(x => x.KodProdazhi == id))
            return ServiceResult.NotFound();

        var error = await Validate(item);
        if (error != null) return ServiceResult.BadRequest(error);

        _context.Entry(item).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var item = await _context.Prodazhi.FindAsync(id);
        if (item == null) return ServiceResult.NotFound();

        _context.Prodazhi.Remove(item);
        await _context.SaveChangesAsync();
        return ServiceResult.Ok();
    }

    // Проверки продажи: дата, наличие клиента, АЗС, топлива
    // и защита: фирма-владелец АЗС должна поставлять это топливо
    private async Task<string?> Validate(Prodazha item)
    {
        if (item.DataProdazhi > DateOnly.FromDateTime(DateTime.Today))
            return "Дата продажи не может быть в будущем";

        if (!await _context.Klienty.AnyAsync(x => x.KartSchet == item.KartSchet))
            return "Клиент с таким карт-счётом не найден";

        var azs = await _context.AzsStancii.FindAsync(item.KodAzs);
        if (azs == null)
            return "АЗС с таким кодом не найдена";

        if (!await _context.Topliva.AnyAsync(x => x.KodTopliva == item.KodTopliva))
            return "Топливо с таким кодом не найдено";

        bool firmaPostavlyaet = await _context.FirmyToplivo
            .AnyAsync(x => x.NazvanieFirmy == azs.NazvanieFirmy && x.KodTopliva == item.KodTopliva);
        if (!firmaPostavlyaet)
            return "Нельзя продать это топливо: фирма-владелец АЗС его не поставляет";

        return null;
    }
}
