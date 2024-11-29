using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LigaDeFutbol.Models;
using Microsoft.AspNetCore.Authorization;

namespace LigaDeFutbol.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class EncuentroController : ControllerBase
    {
        private readonly ContextDb _context;

        public EncuentroController(ContextDb context)
        {
            _context = context;
        }

        // GET: api/Encuentro
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Encuentro>>> ListarEncuentros()
        {
            var encuentros = await _context.Encuentros.Include(e => e.IdCanchaNavigation)
                                                      .Include(e => e.IdFechaNavigation)
                                                      .Include(e => e.DniArbitroNavigation)
                                                      .ToListAsync();
            return Ok(encuentros);
        }

        // GET: api/Encuentro/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Encuentro>> ObtenerEncuentro(int id)
        {
            var encuentro = await _context.Encuentros.Include(e => e.IdCanchaNavigation)
                                                      .Include(e => e.IdFechaNavigation)
                                                      .Include(e => e.DniArbitroNavigation)
                                                      .FirstOrDefaultAsync(e => e.IdEncuentro == id);

            if (encuentro == null)
            {
                return NotFound(new { mensaje = "Encuentro no encontrado." });
            }

            return Ok(encuentro);
        }

        // POST: api/Encuentro
        [HttpPost]
        public async Task<ActionResult<Encuentro>> CrearEncuentro([FromBody] Encuentro encuentro)
        {
            // Validar que la cancha, fecha y árbitro existen
            var canchaExistente = await _context.Canchas.FindAsync(encuentro.IdCancha);
            var fechaExistente = await _context.Fechas.FindAsync(encuentro.IdFecha);
            var arbitroExistente = await _context.Arbitros.FindAsync(encuentro.DniArbitro);

            if (canchaExistente == null)
            {
                return BadRequest(new { mensaje = "La cancha no existe." });
            }

            if (fechaExistente == null)
            {
                return BadRequest(new { mensaje = "La fecha no existe." });
            }

            if (arbitroExistente == null)
            {
                return BadRequest(new { mensaje = "El árbitro no existe." });
            }

            _context.Encuentros.Add(encuentro);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObtenerEncuentro), new { id = encuentro.IdEncuentro }, encuentro);
        }

        // PUT: api/Encuentro/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> ModificarEncuentro(int id, [FromBody] Encuentro encuentro)
        {
            if (id != encuentro.IdEncuentro)
            {
                return BadRequest(new { mensaje = "El ID proporcionado no coincide con el de la entidad encuentro." });
            }

            var encuentroExistente = await _context.Encuentros.FindAsync(id);
            if (encuentroExistente == null)
            {
                return NotFound(new { mensaje = "Encuentro no encontrado para actualizar." });
            }

            // Validar que la cancha, fecha y árbitro existen
            var canchaExistente = await _context.Canchas.FindAsync(encuentro.IdCancha);
            var fechaExistente = await _context.Fechas.FindAsync(encuentro.IdFecha);
            var arbitroExistente = await _context.Arbitros.FindAsync(encuentro.DniArbitro);

            if (canchaExistente == null)
            {
                return BadRequest(new { mensaje = "La cancha no existe." });
            }

            if (fechaExistente == null)
            {
                return BadRequest(new { mensaje = "La fecha no existe." });
            }

            if (arbitroExistente == null)
            {
                return BadRequest(new { mensaje = "El árbitro no existe." });
            }

            // Actualizar los datos del encuentro
            encuentroExistente.IdCancha = encuentro.IdCancha;
            encuentroExistente.FechaHora = encuentro.FechaHora;
            encuentroExistente.GolesEquipo1 = encuentro.GolesEquipo1;
            encuentroExistente.GolesEquipo2 = encuentro.GolesEquipo2;
            encuentroExistente.IdFecha = encuentro.IdFecha;
            encuentroExistente.DniArbitro = encuentro.DniArbitro;

            _context.Entry(encuentroExistente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Encuentros.Any(e => e.IdEncuentro == id))
                {
                    return NotFound(new { mensaje = "Encuentro no encontrado tras el intento de actualización." });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Encuentro/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarEncuentro(int id)
        {
            var encuentro = await _context.Encuentros.FindAsync(id);
            if (encuentro == null)
            {
                return NotFound(new { mensaje = "Encuentro no encontrado para eliminar." });
            }

            _context.Encuentros.Remove(encuentro);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Encuentro eliminado correctamente." });
        }
    }
}
