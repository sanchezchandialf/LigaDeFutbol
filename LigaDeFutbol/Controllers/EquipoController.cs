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

        // GET: Obtener todos los directores técnicos sin equipo
        [HttpGet]
        [Route("tecnicos-sin-equipo")]
        public async Task<IActionResult> ObtenerDirectoresTecnicosSinEquipo()
        {
            var directoresTecnicos = await _context.Set<Persona>()
                .Where(p => p.EsDirectorTecnico && !p.EquipoIdDirectorTecnicoNavigations.Any())
                .Select(p => new
                {
                    p.Id,
                    p.Dni,
                    p.Nombre,
                    p.Apellido,
                    p.Ciudad,
                    p.NTelefono1
                })
                .ToListAsync();

            return Ok(directoresTecnicos);
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
                IdEquipo = request.IdEquipo,
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

        [HttpGet("{id?}")]
        public async Task<IActionResult> ObtenerEquipos(int? id, [FromBody] ObtenerEquiposDTO request)
        {
            var fechaActual = DateOnly.FromDateTime(DateTime.Now);
            var queryEquipos = _context.Equipos.
                Where(e => e.IdTorneoNavigation.IdDivision == request.IdDivision && e.IdTorneoNavigation.IdCategoria == request.IdCategoria);


            if (id != null)
            {
                queryEquipos = queryEquipos.Where(e => e.Id == id);
            };
            if (request.ListarSoloDisponibles == true)
            {
                queryEquipos = queryEquipos.Where(e => fechaActual >= e.IdTorneoNavigation.FechaInicioInscripcion && fechaActual <= e.IdTorneoNavigation.FechaFinalizacionInscripcion);
            };

            var equipos = await queryEquipos
                .Include(e => e.IdDirectorTecnicoNavigation)
                .Include(e => e.IdRepresentanteEquipoNavigation)
                .Include(e => e.IdTorneoNavigation)
                .ToListAsync();

            var mapeado = equipos.Select(e => new EquipoDTO {
            Id=e.Id,
            Nombre=e.Nombre,
            IdDirectorTecnico=e.IdDirectorTecnico,
            IdRepresentanteEquipo=e.IdRepresentanteEquipo,
            IdTorneo=e.IdTorneo,
            Torneo=new TorneoDTO { Id=e.IdTorneoNavigation.Id, 
                Nombre=e.IdTorneoNavigation.Nombre,
                FechaInicio=e.IdTorneoNavigation.FechaInicio, 
                FechaFinalizacion=e.IdTorneoNavigation.FechaFinalizacion,
                FechaInicioInscripcion=e.IdTorneoNavigation.FechaInicioInscripcion,
                FechaFinalizacionInscripcion=e.IdTorneoNavigation.FechaFinalizacionInscripcion},
            DirectorTecnico=new PersonaDTO
            {
                Id = e.IdDirectorTecnicoNavigation.Id,
                Foto = e.IdDirectorTecnicoNavigation.Foto,
                Dni = e.IdDirectorTecnicoNavigation.Dni,
                Nombre = e.IdDirectorTecnicoNavigation.Nombre,
                Apellido = e.IdDirectorTecnicoNavigation.Apellido,
                FechaNacimiento = e.IdDirectorTecnicoNavigation.FechaNacimiento,
                Calle = e.IdDirectorTecnicoNavigation.Calle,
                Numero = e.IdDirectorTecnicoNavigation.Numero,
                Ciudad = e.IdDirectorTecnicoNavigation.Ciudad,
                NTelefono1 = e.IdDirectorTecnicoNavigation.NTelefono1,
                EsJugador = e.IdDirectorTecnicoNavigation.EsJugador,
                EsDirectorTecnico = e.IdDirectorTecnicoNavigation.EsDirectorTecnico,
                EsRepresentanteEquipo = e.IdDirectorTecnicoNavigation.EsRepresentanteEquipo,
                EsRepresentanteAsociacion = e.IdDirectorTecnicoNavigation.EsEncargadoAsociacion,
                NSocio = e.IdDirectorTecnicoNavigation.NSocio,
                IdCategoria = e.IdDirectorTecnicoNavigation.IdCategoria,
                IdDivision = e.IdDirectorTecnicoNavigation.IdDivision
            },
            RepresentanteEquipo=new PersonaDTO
            {
                Id = e.IdDirectorTecnicoNavigation.Id,
                Foto = e.IdRepresentanteEquipoNavigation.Foto,
                Dni = e.IdRepresentanteEquipoNavigation.Dni,
                Nombre = e.IdRepresentanteEquipoNavigation.Nombre,
                Apellido = e.IdRepresentanteEquipoNavigation.Apellido,
                FechaNacimiento = e.IdRepresentanteEquipoNavigation.FechaNacimiento,
                Calle = e.IdRepresentanteEquipoNavigation.Calle,
                Numero = e.IdRepresentanteEquipoNavigation.Numero,
                Ciudad = e.IdRepresentanteEquipoNavigation.Ciudad,
                NTelefono1 = e.IdRepresentanteEquipoNavigation.NTelefono1,
                EsJugador = e.IdRepresentanteEquipoNavigation.EsJugador,
                EsDirectorTecnico = e.IdRepresentanteEquipoNavigation.EsDirectorTecnico,
                EsRepresentanteEquipo = e.IdRepresentanteEquipoNavigation.EsRepresentanteEquipo,
                EsRepresentanteAsociacion = e.IdRepresentanteEquipoNavigation.EsEncargadoAsociacion,
                NSocio = e.IdRepresentanteEquipoNavigation.NSocio,
                IdCategoria = e.IdRepresentanteEquipoNavigation.IdCategoria,
                IdDivision = e.IdRepresentanteEquipoNavigation.IdDivision
            }
            });

            return Ok(mapeado);
        }
    }
}
