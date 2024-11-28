using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LigaDeFutbol.Models;
using Microsoft.AspNetCore.Authorization;

namespace LigaDeFutbol.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class FechaController : ControllerBase
    {
        private readonly ContextDb _context;

        public FechaController(ContextDb context)
        {
            _context = context;
        }

        // GET: api/Fecha
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fecha>>> ListarFechas()
        {
            var fechas = await _context.Fechas.Include(f => f.IdRuedaNavigation)
                                               .ThenInclude(r => r.IdTorneoFkNavigation)
                                               .ToListAsync();
            return Ok(fechas);
        }

        // GET: api/Fecha/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Fecha>> ObtenerFecha(int id)
        {
            var fecha = await _context.Fechas.Include(f => f.IdRuedaNavigation)
                                              .ThenInclude(r => r.IdTorneoFkNavigation)
                                              .FirstOrDefaultAsync(f => f.Id == id);

            if (fecha == null)
            {
                return NotFound(new { mensaje = "Fecha no encontrada." });
            }

            return Ok(fecha);
        }

        // POST: api/Fecha
        [HttpPost]
        public async Task<ActionResult<Fecha>> CrearFecha([FromBody] Fecha fecha)
        {
            // Validar que la rueda existe
            var ruedaExistente = await _context.Rueda.FindAsync(fecha.IdRueda);
            if (ruedaExistente == null)
            {
                return BadRequest(new { mensaje = "La rueda asociada no existe." });
            }

            _context.Fechas.Add(fecha);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObtenerFecha), new { id = fecha.Id }, fecha);
        }

        // PUT: api/Fecha/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> ModificarFecha(int id, [FromBody] Fecha fecha)
        {
            if (id != fecha.Id)
            {
                return BadRequest(new { mensaje = "El ID proporcionado no coincide con el de la fecha." });
            }

            var fechaExistente = await _context.Fechas.FindAsync(id);
            if (fechaExistente == null)
            {
                return NotFound(new { mensaje = "Fecha no encontrada para actualizar." });
            }

            // Actualizar los campos según sea necesario
            fechaExistente.IdRueda = fecha.IdRueda;

            _context.Entry(fechaExistente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Fechas.Any(f => f.Id == id))
                {
                    return NotFound(new { mensaje = "Fecha no encontrada tras el intento de actualización." });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Fecha/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarFecha(int id)
        {
            var fecha = await _context.Fechas.FindAsync(id);
            if (fecha == null)
            {
                return NotFound(new { mensaje = "Fecha no encontrada para eliminar." });
            }

            _context.Fechas.Remove(fecha);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Fecha eliminada correctamente." });
        }
    }
}
