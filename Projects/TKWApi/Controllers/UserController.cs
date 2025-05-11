using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TKWData;
using TKWData.Models;
using TKWLogic;

namespace TKWApi.Controllers;

[ApiController]
[Route("user")]
public class UserController : TKWAPIControllerBase
{
    private readonly TKWDbContext _context;

    public UserController(ICommandHandlerResolver commandDispatcher, TKWDbContext context) : base(commandDispatcher)
    {
        _context = context;
    }

    [HttpPost]
    [Route("RegisterUser")]
    //public async Task<Created<UserDTO>> RegisterUser([FromBody] RegisterUserCommand command)
    public async Task<Created> RegisterUser([FromBody] RegisterUserCommand command)
    {

        CurrentCommand = command;
        CommandDispatcher.HandleCommand(CurrentCommand as RegisterUserCommand, new TKWAppContext(new TKWWebApiContextCreator(_context)));

        //return TypedResults.Created($"/todos/{user.Id}", user.AsUserDTO());
        return TypedResults.Created();
    }
}
