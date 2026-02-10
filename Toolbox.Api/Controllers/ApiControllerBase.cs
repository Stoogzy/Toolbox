using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Toolbox.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender? _mediator;

    /// <summary>
    /// Lazy-loads the MediatR Sender service. 
    /// This property uses the "Null-coalescing assignment operator" (??=).
    /// If _mediator is null, it fetches the service from the HttpContext; 
    /// otherwise, it returns the existing instance.
    /// This means we only ask the DI container for the MediatR service the moment we need it.
    /// </summary>
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
