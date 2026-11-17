namespace AzsApi.Services;

public enum ResultStatus
{
    Ok,
    NotFound,
    BadRequest,
    Conflict
}

// Результат операции сервиса: контроллер по нему выбирает код ответа
public class ServiceResult
{
    public ResultStatus Status { get; private set; } = ResultStatus.Ok;
    public string? Message { get; private set; }
    public bool IsOk => Status == ResultStatus.Ok;

    public static ServiceResult Ok() => new ServiceResult();

    public static ServiceResult NotFound(string? message = null) =>
        new ServiceResult { Status = ResultStatus.NotFound, Message = message };

    public static ServiceResult BadRequest(string message) =>
        new ServiceResult { Status = ResultStatus.BadRequest, Message = message };

    public static ServiceResult Conflict(string message) =>
        new ServiceResult { Status = ResultStatus.Conflict, Message = message };
}
