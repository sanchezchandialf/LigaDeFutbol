using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LigaDeFutbol.Models;
using LigaDeFutbol.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace LigaDeFutbol.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ContextDb _context;

        public CategoriaController(ContextDb context)
        {
            _context = context;
        }

        // GET: api/Categoria
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> ListarCategorias()
        {
            try
            {
                var categorias = await _context.Categorias
                                               .Select(c => new CategoriaDTO
                                               {
                                                   Id = c.Id,
                                                   EdadMinima = c.EdadMinima,
                                                   EdadMaxima = c.EdadMaxima,
                                                   Nombre = c.Nombre
                                               })
                                               .ToListAsync();

                return Ok(categorias);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error al obtener las categorías.", error = ex.Message });
            }
        }

        // GET: api/Categoria/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaDTO>> ObtenerCategoria(int id)
        {
            try
            {
                var categoria = await _context.Categorias
                                              .Where(c => c.Id == id)
                                              .Select(c => new CategoriaDTO
                                              {
                                                  Id = c.Id,
                                                  EdadMinima = c.EdadMinima,
                                                  EdadMaxima = c.EdadMaxima,
                                                  Nombre = c.Nombre
                                              })
                                              .FirstOrDefaultAsync();

                if (categoria == null)
                {
                    return NotFound(new { mensaje = "Categoría no encontrada." });
                }

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error al obtener la categoría.", error = ex.Message });
            }
        }
    }
}
