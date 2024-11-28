using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LigaDeFutbol.Models;
using LigaDeFutbol.Models.DTOs;

namespace LigaDeFutbol.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DivisionController : ControllerBase
    {
        private readonly ContextDb _context;

        public DivisionController(ContextDb context)
        {
            _context = context;
        }

        // GET: api/Division
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DivisionDTO>>> ListarDivisiones()
        {
            try
            {
                var divisiones = await _context.Divisiones
                                               .Select(d => new DivisionDTO
                                               {
                                                   Id = d.Id,
                                                   Nombre = d.Nombre
                                               })
                                               .ToListAsync();

                return Ok(divisiones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error al obtener las divisiones.", error = ex.Message });
            }
        }

        // GET: api/Division/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<DivisionDTO>> ObtenerDivision(int id)
        {
            try
            {
                var division = await _context.Divisiones
                                             .Where(d => d.Id == id)
                                             .Select(d => new DivisionDTO
                                             {
                                                 Id = d.Id,
                                                 Nombre = d.Nombre
                                             })
                                             .FirstOrDefaultAsync();

                if (division == null)
                {
                    return NotFound(new { mensaje = "División no encontrada." });
                }

                return Ok(division);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error al obtener la división.", error = ex.Message });
            }
        }
    }
}
