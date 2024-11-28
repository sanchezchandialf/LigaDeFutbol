using LigaDeFutbol.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LigaDeFutbol.Dtos;
using LigaDeFutbol.Models;
using Microsoft.AspNetCore.Authorization;
namespace LigaDeFutbol.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TorneosController : ControllerBase
    {
        private readonly DbContext _context;

        public TorneosController(DbContext context)
        {
            _context = context;
        }

        // Crear un nuevo torneo
        [HttpPost]
        [Authorize]
        [Route("crear")]
        public async Task<IActionResult> CrearTorneo([FromBody] CrearTorneoDTO modelo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoria = await _context.Set<Categoria>().FindAsync(modelo.IdCategoria);
            var division = await _context.Set<Divisione>().FindAsync(modelo.IdDivision);

            if (categoria == null || division == null)
            {
                return NotFound("Categoría o División no encontrada.");
            }

            var nuevoTorneo = new Torneo
            {
                Nombre = modelo.Nombre,
                FechaInicio = modelo.FechaInicio,
                FechaFinalizacion = modelo.FechaFinalizacion,
                FechaInicioInscripcion = modelo.FechaInicioInscripcion,
                FechaFinalizacionInscripcion = modelo.FechaFinalizacionInscripcion,
                IdEncargadoAsociacion = modelo.IdEncargadoAsociacion,
                IdCategoria = modelo.IdCategoria,
                IdDivision = modelo.IdDivision,
            };

            _context.Add(nuevoTorneo);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Torneo creado con éxito", torneoId = nuevoTorneo.Id });
        }
    }
}