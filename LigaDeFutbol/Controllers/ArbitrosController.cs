using Microsoft.AspNetCore.Mvc;
using LigaDeFutbol.Models;
using LigaDeFutbol.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace LigaDeFutbol.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ArbitrosController : ControllerBase
    {
        private readonly ContextDb _context;

        public ArbitrosController(ContextDb context)
        {
            _context = context;
        }

        // GET: api/Arbitros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArbitroDto>>> ListarArbitros()
        {
            var arbitros = await _context.Arbitros
                .Include(a => a.IdExperienciaNavigation)
                .Select(a => new ArbitroDto
                {
                    DniArbitro = a.DniArbitro,
                    Nombre = a.Nombre!,
                    Apellido = a.Apellido!,
                    Ciudad = a.Ciudad,
                    EsCertificado = a.EsCertificado,
                    IdExperiencia = a.IdExperiencia
                })
                .ToListAsync();

            return Ok(arbitros);
        }

        // GET: api/Arbitros/{dni}
        [HttpGet("{dni}")]
        public async Task<ActionResult<ArbitroDto>> ObtenerArbitro(string dni)
        {
            var arbitro = await _context.Arbitros
                .Include(a => a.IdExperienciaNavigation)
                .Where(a => a.DniArbitro == dni)
                .Select(a => new ArbitroDto
                {
                    DniArbitro = a.DniArbitro,
                    Nombre = a.Nombre!,
                    Apellido = a.Apellido!,
                    Ciudad = a.Ciudad,
                    EsCertificado = a.EsCertificado,
                    IdExperiencia = a.IdExperiencia
                })
                .FirstOrDefaultAsync();

            if (arbitro == null)
            {
                return NotFound(new { mensaje = "Árbitro no encontrado." });
            }

            return Ok(arbitro);
        }

        // POST: api/Arbitros
        [HttpPost]
        public async Task<ActionResult<ArbitroDto>> CrearArbitro(ArbitroDto arbitroDto)
        {
            // Validar si ya existe un árbitro con el mismo DNI
            if (await _context.Arbitros.AnyAsync(a => a.DniArbitro == arbitroDto.DniArbitro))
            {
                return BadRequest(new { mensaje = "Ya existe un árbitro con el mismo DNI." });
            }

            var nuevoArbitro = new Arbitro
            {
                DniArbitro = arbitroDto.DniArbitro,
                Nombre = arbitroDto.Nombre,
                Apellido = arbitroDto.Apellido,
                Ciudad = arbitroDto.Ciudad,
                EsCertificado = arbitroDto.EsCertificado,
                IdExperiencia = arbitroDto.IdExperiencia
            };

            _context.Arbitros.Add(nuevoArbitro);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObtenerArbitro), new { dni = nuevoArbitro.DniArbitro }, arbitroDto);
        }

        // PUT: api/Arbitros/{dni}
        [HttpPut("{dni}")]
        public async Task<IActionResult> ModificarArbitro(string dni, ArbitroDto arbitroDto)
        {
            if (dni != arbitroDto.DniArbitro)
            {
                return BadRequest(new { mensaje = "El DNI proporcionado no coincide con el del árbitro." });
            }

            var arbitro = await _context.Arbitros.FindAsync(dni);
            if (arbitro == null)
            {
                return NotFound(new { mensaje = "Árbitro no encontrado para actualizar." });
            }

            // Actualizar campos
            arbitro.Nombre = arbitroDto.Nombre;
            arbitro.Apellido = arbitroDto.Apellido;
            arbitro.Ciudad = arbitroDto.Ciudad;
            arbitro.EsCertificado = arbitroDto.EsCertificado;
            arbitro.IdExperiencia = arbitroDto.IdExperiencia;

            _context.Entry(arbitro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Arbitros/{dni}
        [HttpDelete("{dni}")]
        public async Task<IActionResult> EliminarArbitro(string dni)
        {
            var arbitro = await _context.Arbitros.FindAsync(dni);
            if (arbitro == null)
            {
                return NotFound(new { mensaje = "Árbitro no encontrado para eliminar." });
            }

            _context.Arbitros.Remove(arbitro);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Árbitro eliminado correctamente." });
        }
    }
}
