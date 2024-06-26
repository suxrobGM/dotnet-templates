﻿using Company.Project.Application.Commands;
using Company.Project.Application.Queries;
using Company.Project.Core.Cqrs;
using Company.Project.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Company.Project.API.Controllers;

[Route("users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Result<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserById(string id)
    {
        var result = await _mediator.Send(new GetUserQuery {Id = id});
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUsersList([FromQuery] GetUsersQuery query)
    {
        var result = await _mediator.Send(query);
        return ((Result)result).Success ? Ok(result) : BadRequest(result);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand request)
    {
        var result = await _mediator.Send(request);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUserDetails(string id, [FromBody] UpdateUserCommand request)
    {
        request.Id = id;
        var result = await _mediator.Send(request);
        return result.Success ? Ok(result) : BadRequest(result);
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var result = await _mediator.Send(new DeleteUserCommand {Id = id});
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
