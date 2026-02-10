using MediatR;
using Microsoft.AspNetCore.Mvc;
using Toolbox.Application.Employees.Command.Create;
using Toolbox.Application.Employees.Command.Update;
using Toolbox.Application.Employees.Dtos;
using Toolbox.Application.Employees.Queries.GetAll;
using Toolbox.Application.Employees.Queries.GetById;

namespace Toolbox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController(ISender mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<EmployeeDto>>> GetAllEmployees()
    {
        List<EmployeeDto> result = await mediator.Send(new GetAllEmployeesQuery());

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EmployeeDto>> CreateEmployee(CreateEmployeeCommand command)
    {
        EmployeeDto employeeDto = await mediator.Send(command);

        return StatusCode(StatusCodes.Status201Created, employeeDto);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EmployeeDto>> GetById(Guid id)
    {
        return Ok(await mediator.Send(new GetEmployeeByIdQuery(id)));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, UpdateEmployeeCommand command)
    {
        if (id != command.Id) return BadRequest();
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await Mediator.Send(new DeleteEmployeeCommand(id));
        return NoContent();
    }
}
