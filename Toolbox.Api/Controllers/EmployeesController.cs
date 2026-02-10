using MediatR;
using Microsoft.AspNetCore.Mvc;
using Toolbox.Application.Employees.Command.Create;
using Toolbox.Application.Employees.Command.Delete;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeDto>> GetById(Guid id)
    {
        EmployeeDto employeeDto = await mediator.Send(new GetEmployeeByIdQuery(id));

        return StatusCode(StatusCodes.Status200OK, employeeDto);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(Guid id, UpdateEmployeeCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID in URL does not match ID in request body.");
        }

        await mediator.Send(command);

        // Standard response for a successful update with no returned body
        return StatusCode(StatusCodes.Status204NoContent);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(Guid id)
    {
        await mediator.Send(new DeleteEmployeeCommand(id));

        return StatusCode(StatusCodes.Status204NoContent);
    }
}
