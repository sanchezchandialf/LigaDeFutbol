using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LigaDeFutbol.Models;
using LigaDeFutbol.DTos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
namespace LigaDeFutbol.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonasController : ControllerBase
    {
        private readonly ContextDb _context;

        public PersonasController(ContextDb context)
        {
            _context = context;
        }

        // Endpoint para registrar un jugador
        [HttpPost()]
        
        public async Task<IActionResult> RegistrarPersona([FromBody] RegistrarPersonaDTO request)
        {
            if (request == null)
                return BadRequest("Datos inválidos");

            // Crear un objeto Persona
            var persona = new Persona
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                FechaNacimiento = request.FechaNacimiento,
                Dni = request.Dni,
                Contraseña = request.Contraseña,
                Calle = request.Calle,
                Numero = request.Numero,
                Ciudad = request.Ciudad,
                NTelefono1 = request.NTelefono1,

       
              
               

            };

            // Validar y asignar categoría según la edad mateo hdp

            /*int edad = CalcularEdad(request.FechaNacimiento);
            persona.NSocio = ObtenerCategoriaPorEdad(edad);
            */
            if (request.EsJugador==true) {
                persona.EsJugador = request.EsJugador;
                persona.NSocio=Guid.NewGuid().ToString(); 
                int divisionId=await ObtenerDivision(request);
                if (divisionId == 0)
                {
                    return BadRequest("No se encontró la division del jugador.");
                }
                persona.IdDivision = divisionId;
                int categoriaId=await ObtenerCategoriaPorEdad(request);
                if (categoriaId == 0)
                {
                    return BadRequest("No se encontró una categoría adecuada para el jugador.");
                }
                persona.IdCategoria = categoriaId;
                persona.Foto = request.Foto;
                


            }
            if (request.EsDirectorTecnico==true) { persona.EsDirectorTecnico = request.EsDirectorTecnico; }
            if (request.EsRepresentanteEquipo==true) { persona.EsRepresentanteEquipo = request.EsRepresentanteEquipo; }


            

            await _context.Personas.AddAsync(persona);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Mensaje = "Jugador registrado exitosamente",
                //Categoria = persona.NSocio
            });
        }
       

        private async Task<int> ObtenerCategoriaPorEdad(RegistrarPersonaDTO request)
        {
           int edadjugador= CalcularEdad(request , request.FechaNacimiento);
        
            var categoria= await _context.Categorias.FirstOrDefaultAsync(c => c.EdadMinima <= edadjugador && c.EdadMaxima >= edadjugador);
            if (categoria != null)
            {
                return categoria.Id;
            }
            else
            {
                return 0;
            }
            
            
           
        }
        private async Task<int> ObtenerDivision(RegistrarPersonaDTO request)
        {
             var division= await _context.Divisiones.FirstOrDefaultAsync(c => c.Id == request.IdDivision);
            if (division != null)
            {
                return division.Id;
            }
            else
            {
                return 0;
            }
        }

        private int CalcularEdad( RegistrarPersonaDTO request,DateTime fechaNacimiento)
        {
            int edad = DateTime.Now.Year - request.FechaNacimiento.Year  ;
            if (DateTime.Now.DayOfYear < request.FechaNacimiento.DayOfYear)
                edad--;
            return edad;
        }
        
    }
}
