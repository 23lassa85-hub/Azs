namespace AzsApi.Services;

// Общий набор операций для сущностей с одним ключом: T — сущность, K — тип ключа
public interface ICommonService<T, K>
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(K id);
    Task<ServiceResult> CreateAsync(T item);
    Task<ServiceResult> UpdateAsync(K id, T item);
    Task<ServiceResult> DeleteAsync(K id);
}
