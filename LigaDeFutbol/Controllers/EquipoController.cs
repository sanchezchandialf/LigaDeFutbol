using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using LigaDeFutbol.Models;
using LigaDeFutbol.DTos;
using Microsoft.EntityFrameworkCore;
using LigaDeFutbol.Dtos;

namespace LigaDeFutbol.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipoController : ControllerBase
    {
        private readonly DbLigaContext _context;

        public EquipoController(DbLigaContext context)
        {
            _context = context;
        }

        // Endpoint para registrar un equipo (ya definido antes)
        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarEquipo([FromBody] RegistrarEquipoDTO request)
        {
            if (request == null)
                return BadRequest("Datos inválidos");

            // Validaciones para Director Técnico y Representante
            var directorTecnico = await _context.Personas.FindAsync(request.IdDirectorTecnico);
            if (directorTecnico == null || !directorTecnico.EsDirectorTecnico)
                return BadRequest("El Director Técnico especificado no es válido.");

            var representanteEquipo = await _context.Personas.FindAsync(request.IdRepresentanteEquipo);
            if (representanteEquipo == null || !representanteEquipo.EsRepresentanteEquipo)
                return BadRequest("El Representante del Equipo especificado no es válido.");

            // Crear el equipo
            var equipo = new Equipo
            {
                Nombre = request.Nombre,
                IdDirectorTecnico = request.IdDirectorTecnico,
                IdRepresentanteEquipo = request.IdRepresentanteEquipo,
                IdDirectorTecnicoNavigation = directorTecnico,
                IdRepresentanteEquipoNavigation = representanteEquipo
            };

            await _context.Equipos.AddAsync(equipo);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Mensaje = "Equipo registrado exitosamente",
                EquipoId = equipo.Id,
                Nombre = equipo.Nombre
            });
        }

        // Endpoint para asignar jugadores a un equipo
        [HttpPost("asignar-jugadores")]
        public async Task<IActionResult> AsignarJugadores([FromBody] AsignarJugadoresDTO request)
        {
            // Validar equipo
            var equipo = await _context.Equipos
                .Include(e => e.JugadorEquipos)
                .FirstOrDefaultAsync(e => e.Id == request.IdEquipo);

            if (equipo == null)
                return NotFound("Equipo no encontrado.");

            // Validar jugadores
            var jugadores = await _context.Personas
                .Where(p => request.IdJugadores.Contains(p.Id) && p.EsJugador)
                .ToListAsync();

            if (jugadores.Count != request.IdJugadores.Count)
                return BadRequest("Algunos jugadores no existen o no son válidos.");

            // Asignar jugadores al equipo
            foreach (var jugador in jugadores)
            {
                // Evitar duplicados
                if (!equipo.JugadorEquipos.Any(je => je.IdJugador == jugador.Id))
                {
                    equipo.JugadorEquipos.Add(new JugadorEquipo
                    {
                        IdEquipo = equipo.Id,
                        IdJugador = jugador.Id,
                        
                    });
                }
            }

            // Guardar cambios
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Mensaje = "Jugadores asignados al equipo exitosamente",
                EquipoId = equipo.Id,
                NombreEquipo = equipo.Nombre,
                JugadoresAsignados = jugadores.Select(j => new
                {
                    j.Id,
                    NombreCompleto = $"{j.Nombre} {j.Apellido}"
                })
            });
        }

        // Endpoint para listar jugadores de un equipo
        [HttpGet("{equipoId}/jugadores")]
        public async Task<IActionResult> ListarJugadoresPorEquipo(int equipoId)
        {
            var equipo = await _context.Equipos
                .Include(e => e.JugadorEquipos)
                .ThenInclude(je => je.IdJugadorNavigation)
                .FirstOrDefaultAsync(e => e.Id == equipoId);

            if (equipo == null)
                return NotFound("Equipo no encontrado.");

            var jugadores = equipo.JugadorEquipos
                .Select(je => new
                {
                    Id = je.IdJugador,
                    NombreCompleto = $"{je.IdJugadorNavigation.Nombre} {je.IdJugadorNavigation.Apellido}",
                    
                })
                .ToList();

            return Ok(new
            {
                EquipoId = equipo.Id,
                NombreEquipo = equipo.Nombre,
                Jugadores = jugadores
            });
        }
    }
}
