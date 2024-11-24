using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LigaDeFutbol.Models;
using LigaDeFutbol.DTos; 
namespace LigaDeFutbol.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonasController : ControllerBase
    {
        private readonly DbLigaContext _context;

        public PersonasController(DbLigaContext context)
        {
            _context = context;
        }

        // Endpoint para registrar un jugador
        [HttpPost()]
        public async Task<IActionResult> RegistrarJugador([FromBody] RegistroJugadorDTO request)
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
                Foto = request.Foto,
                Calle = request.Calle,
                Numero = request.Numero,
                Ciudad = request.Ciudad,
                NTelefono1 = request.NTelefono1,
                NTelefono2 = request.NTelefono2,
              
               

            };

            // Validar y asignar categoría según la edad mateo hdp

            /*int edad = CalcularEdad(request.FechaNacimiento);
            persona.NSocio = ObtenerCategoriaPorEdad(edad);
            */
            if (request.EsJugador==true) { persona.EsJugador = request.EsJugador; persona.NSocio=Guid.NewGuid().ToString(); }
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
        /*
        private int CalcularEdad(DateTime fechaNacimiento)
        {
            var hoy = DateTime.Today;
            int edad = hoy.Year - fechaNacimiento.Year;

            if (fechaNacimiento.Date > hoy.AddYears(-edad))
                edad--;

            return edad;
        }

        private string ObtenerCategoriaPorEdad(int edad)
        {
            if (edad >= 41 && edad <= 45)
                return "Maxi";
            if (edad >= 46 && edad <= 50)
                return "Super";
            if (edad >= 51 && edad <= 55)
                return "Master";
            return "Senior";
        }
        */
    }
}
