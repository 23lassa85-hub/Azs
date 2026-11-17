using AzsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzsApi.Controllers;

// Общая часть контроллеров: превращает результат сервиса в ответ API
public abstract class ServiceControllerBase : ControllerBase
{
    protected ActionResult ToActionResult(ServiceResult result)
    {
        return result.Status switch
        {
            ResultStatus.NotFound => NotFound(result.Message),
            ResultStatus.BadRequest => BadRequest(result.Message),
            ResultStatus.Conflict => Conflict(result.Message),
            _ => NoContent()
        };
    }
}
