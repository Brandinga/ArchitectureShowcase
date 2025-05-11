using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TKWData;
using TKWData.Models;

namespace TKWApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayerController : ControllerBase
{
    private readonly TKWDbContext _context;

    public PlayerController(TKWDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Player>>> GetPlayerItems()
    {
        return await _context.Player
            .Select(x => x)
            .ToListAsync();
    }

}
