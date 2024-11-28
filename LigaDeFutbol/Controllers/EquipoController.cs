using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using LigaDeFutbol.Models;
using LigaDeFutbol.DTos;
using Microsoft.EntityFrameworkCore;
using LigaDeFutbol.Dtos;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;

namespace LigaDeFutbol.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class EquipoController : ControllerBase
    {
        private readonly ContextDb _context;

        public EquipoController(ContextDb context)
        {
            _context = context;
        }

        // Endpoint para registrar un equipo (ya definido antes)
        [HttpPost()]
        public async Task<IActionResult> RegistrarEquipo([FromBody] RegistrarEquipoDTO request)
        {
            if (request == null)
                return BadRequest("Datos inválidos");

            // Validaciones para Director Técnico y Representante
            var directorTecnico = await _context.Personas.FindAsync(request.IdDirectorTecnico);
            if (directorTecnico == null || !directorTecnico.EsDirectorTecnico)
                return BadRequest("El Director Técnico especificado no es válido.");

            var equipotecnico=await _context.Equipos.FirstOrDefaultAsync(e=>e.Id==request.IdDirectorTecnico);

            if (equipotecnico != null)
                return BadRequest("El Director Técnico especificado ya tiene un equipo asignado.");

            var representanteEquipo = await _context.Personas.FindAsync(request.IdRepresentanteEquipo);
            if (representanteEquipo == null || !representanteEquipo.EsRepresentanteEquipo)
                return BadRequest("El Representante del Equipo especificado no es válido.");

            var torneo=  await _context.Torneos.FirstOrDefaultAsync(t=>t.Id==request.IdTorneo);
            if (torneo == null)
                return BadRequest("El Torneo especificado no es valido.");

            DateOnly ahora= DateOnly.FromDateTime(DateTime.Now);
            if (ahora<torneo.FechaInicioInscripcion|| ahora>torneo.FechaFinalizacionInscripcion)  
                return BadRequest("El Torneo especificado no es valido.");
            var equipoRepetido = await _context.Equipos.FirstOrDefaultAsync(e => (e.Nombre == request.Nombre || e.IdRepresentanteEquipo == request.IdRepresentanteEquipo || e.IdDirectorTecnico  == request.IdDirectorTecnico) && e.IdTorneo == request.IdTorneo);
            
            if(equipoRepetido != null)
                return BadRequest("Ya existe un equipo en el torneo que tenga el mismo nombre o director tecnico o representante y pertenece al torneo.");
            
            // Crear el equipo
            var equipo = new Equipo
            {
                Nombre = request.Nombre,
                IdDirectorTecnico = request.IdDirectorTecnico,
                IdRepresentanteEquipo = request.IdRepresentanteEquipo,
                IdDirectorTecnicoNavigation = directorTecnico,
                IdRepresentanteEquipoNavigation = representanteEquipo,
                IdTorneo =  request.IdTorneo
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
        [HttpPost("{equipoId}/asignar-jugadores")]
        [Authorize]
        public async Task<IActionResult> AsignarJugadores(int equipoId,[FromBody] AsignarJugadoresDTO request)
        {
            
            
            
            
            // Validar jugador
            var jugador = await _context.Personas
              .FirstOrDefaultAsync(p => request.IdJugador == p.Id);


            if (jugador == null)
                return BadRequest("El jugador no existe.");








            // Validar equipo
            var equipo = await _context.Equipos
                .FirstOrDefaultAsync(e => e.Id == equipoId);

            if (equipo == null)
                return NotFound("Equipo no encontrado."); 

          
           
          
            var torneo= await _context.Torneos.FirstOrDefaultAsync(t=>t.Id==equipo.IdTorneo);
            if (torneo == null)
                return BadRequest("El Torneo especificado no es valido.");
        

            //validar que un jugador solo pueda estar en un equipo en un torneo
            var equiposEnTorneo = await _context.Equipos.Where(e => e.IdTorneo == torneo.Id).ToListAsync(); //equipos en torneo=
            
            foreach (var equipoEnTorneo in equiposEnTorneo)
            {
                foreach (var jugadorEquipo in equipoEnTorneo.JugadorEquipos)
                {
                    if (request.IdJugador==jugadorEquipo.IdJugador)
                    {
                        return BadRequest("El jugador ya esta anotado en otro equipo en un torneo");
                    }
                }
            }

            if (torneo.IdDivision!=jugador.IdDivision) 
            {
                return BadRequest("El jugador no pertenece a la division del torneo");
            }
    
            if(torneo.IdCategoria!=jugador.IdCategoria)
                return BadRequest("El jugador no pertenece a la categoria del torneo");

            var jugadorequipo = new JugadorEquipo
            {
                IdJugador = request.IdJugador,
                IdEquipo = request.equipoId,
                Aprobado = true

            };
          
            // Guardar cambios
            await _context.SaveChangesAsync();

          return Ok(new
          {
              Mensaje = "Jugador asignado exitosamente al equipo"
          });
        }



        // Endpoint para listar jugadores de un equipo
        [HttpGet("{equipoId}/jugadores")]
        public async Task<IActionResult> ListarJugadoresPorEquipo(int equipoId)
        {
            var equipo = await _context.Equipos
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
