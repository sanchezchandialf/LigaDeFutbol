using System;
using System.Linq;
using System.Threading.Tasks;
using LigaDeFutbol.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LigaDeFutbol.Dtos;


namespace LigaDeFutbol.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TorneosController : ControllerBase
    {
        private readonly ContextDb _context;

        public TorneosController(ContextDb context)
        {
            _context = context;
        }

        // Crear un nuevo torneo
        [HttpPost()]
        [Authorize]
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

        // GET: Obtener todos los torneos
        [HttpGet()]
        [Authorize]
        public async Task<IActionResult> ObtenerTodosLosTorneos()
        {
            var torneos = await _context.Set<Torneo>()
                .Select(t => new
                {
                    t.Id,
                    t.Nombre,
                    t.FechaInicio,
                    t.FechaFinalizacion,
                    t.FechaInicioInscripcion,
                    t.FechaFinalizacionInscripcion,
                    t.IdCategoria,
                    t.IdDivision,
                    CategoriaNombre = t.IdCategoriaNavigation.Nombre,
                    DivisionNombre = t.IdDivisionNavigation.Nombre
                })
                .ToListAsync();

            return Ok(torneos);
        }

        // GET: Obtener torneos con fecha de inscripción activa
        [HttpGet("inscripcion-activa")]
        [Authorize]
        public async Task<IActionResult> ObtenerTorneosConInscripcionActiva()
        {
            DateOnly hoy = DateOnly.FromDateTime(DateTime.Now);

            var torneosActivos = await _context.Set<Torneo>()
                .Where(t => hoy >= t.FechaInicioInscripcion && hoy <= t.FechaFinalizacionInscripcion)
                .Include(e => e.IdCategoriaNavigation)
                .Include(e => e.IdDivisionNavigation)
                .ToListAsync();

            var dto = torneosActivos.Select(e => new TorneoDTO
            {
                Id = e.Id,
                Nombre = e.Nombre,
                FechaInicio = e.FechaInicio,
                FechaFinalizacion = e.FechaFinalizacion,
                FechaInicioInscripcion = e.FechaInicioInscripcion,
                FechaFinalizacionInscripcion = e.FechaFinalizacionInscripcion,
                IdDivision = e.IdDivision,
                IdCategoria = e.IdCategoria,
                Categoria = new Models.DTOs.CategoriaDTO { 
                Id= e.IdCategoriaNavigation.Id,
                Nombre = e.IdCategoriaNavigation.Nombre,
                EdadMaxima = e.IdCategoriaNavigation.EdadMaxima,
                EdadMinima = e.IdCategoriaNavigation.EdadMinima},
                Division = new Models.DTOs.DivisionDTO
                {
                    Id = e.IdDivisionNavigation.Id,
                    Nombre = e.IdDivisionNavigation.Nombre
                }
            });

            return Ok(dto);
        }
    }
}