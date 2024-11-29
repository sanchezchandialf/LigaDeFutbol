using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LigaDeFutbol.Models;
using LigaDeFutbol.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace LigaDeFutbol.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CanchasController : ControllerBase
    {
        private readonly ContextDb _context;

        public CanchasController(ContextDb context)
        {
            _context = context;
        }

        // GET: api/Canchas
        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<CanchaDto>>> ListarCanchas()
        {
            var canchas = await _context.Canchas
                .Select(c => new CanchaDto
                {
                    IdCancha = c.IdCancha,
                    Nombre = c.Nombre,
                    Calle = c.Calle,
                    Numero = c.Numero,
                    Ciudad = c.Ciudad
                })
                .ToListAsync();

            return Ok(canchas);
        }

        // GET: api/Canchas/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CanchaDto>> ObtenerCancha(int id)
        {
            var cancha = await _context.Canchas
                .Where(c => c.IdCancha == id)
                .Select(c => new CanchaDto
                {
                    IdCancha = c.IdCancha,
                    Nombre = c.Nombre,
                    Calle = c.Calle,
                    Numero = c.Numero,
                    Ciudad = c.Ciudad
                })
                .FirstOrDefaultAsync();

            if (cancha == null)
            {
                return NotFound(new { mensaje = "Cancha no encontrada." });
            }

            return Ok(cancha);
        }

        // POST: api/Canchas
        [HttpPost]
        public async Task<ActionResult<CanchaDto>> CrearCancha([FromBody] CanchaDto canchaDto)
        {
            var nuevaCancha = new Cancha
            {
                Nombre = canchaDto.Nombre,
                Calle = canchaDto.Calle,
                Numero = canchaDto.Numero,
                Ciudad = canchaDto.Ciudad
            };

            _context.Canchas.Add(nuevaCancha);
            await _context.SaveChangesAsync();

            canchaDto.IdCancha = nuevaCancha.IdCancha; // Actualizamos el ID generado por la base de datos

            return CreatedAtAction(nameof(ObtenerCancha), new { id = nuevaCancha.IdCancha }, canchaDto);
        }

        // PUT: api/Canchas/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> ModificarCancha(int id, [FromBody] CanchaDto canchaDto)
        {
            if (id != canchaDto.IdCancha)
            {
                return BadRequest(new { mensaje = "El ID de la cancha no coincide." });
            }

            var cancha = await _context.Canchas.FindAsync(id);
            if (cancha == null)
            {
                return NotFound(new { mensaje = "Cancha no encontrada." });
            }

            // Actualizamos los campos
            cancha.Nombre = canchaDto.Nombre;
            cancha.Calle = canchaDto.Calle;
            cancha.Numero = canchaDto.Numero;
            cancha.Ciudad = canchaDto.Ciudad;

            _context.Entry(cancha).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Canchas.Any(c => c.IdCancha == id))
                {
                    return NotFound(new { mensaje = "Cancha no encontrada tras el intento de actualización." });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Canchas/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCancha(int id)
        {
            var cancha = await _context.Canchas.FindAsync(id);
            if (cancha == null)
            {
                return NotFound(new { mensaje = "Cancha no encontrada." });
            }

            _context.Canchas.Remove(cancha);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Cancha eliminada correctamente." });
        }
    }
}
