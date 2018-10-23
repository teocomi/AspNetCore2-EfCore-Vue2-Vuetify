using AspNetCore.Sample.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Sample.Controllers
{
  // AUTHORIZATION
  //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme + "," + CookieAuthenticationDefaults.AuthenticationScheme)]
  [Route("api/[controller]")]
  [ApiController]
  public class RoomsController : ControllerBase
  {
    private readonly DataContext _context;

    public RoomsController(DataContext context)
    {
      _context = context;
    }

    // GET: api/Rooms
    [HttpGet]
    public IEnumerable<Room> GetRooms()
    {
      return _context.Rooms;
    }

    // GET: api/Rooms/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoom([FromRoute] int id)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var room = await _context.Rooms.FindAsync(id);

      if (room == null)
      {
        return NotFound();
      }

      return Ok(room);
    }

    // PUT: api/Rooms/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutRoom([FromRoute] int id, [FromBody] Room room)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      if (id != room.Id)
      {
        return BadRequest();
      }

      _context.Entry(room).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!RoomExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return NoContent();
    }

    // POST: api/Rooms
    [HttpPost]
    public async Task<IActionResult> PostRoom([FromBody] Room room)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      _context.Rooms.Add(room);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetRoom", new { id = room.Id }, room);
    }

    // DELETE: api/Rooms/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoom([FromRoute] int id)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var room = await _context.Rooms.FindAsync(id);
      if (room == null)
      {
        return NotFound();
      }

      _context.Rooms.Remove(room);
      await _context.SaveChangesAsync();

      return Ok(room);
    }

    private bool RoomExists(int id)
    {
      return _context.Rooms.Any(e => e.Id == id);
    }
  }
}