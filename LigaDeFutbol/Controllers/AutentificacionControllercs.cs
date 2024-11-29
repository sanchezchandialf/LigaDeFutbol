using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LigaDeFutbol.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using LigaDeFutbol.Dtos;
using Microsoft.Extensions.Configuration; // agregado
using Microsoft.EntityFrameworkCore; // Agrega esta directiva using
namespace LigaDeFutbol.Controllers
{
    [Route("api/[controller]")]
    
    [ApiController]
    public class AutentificacionController : ControllerBase // corregido el nombre de la clase
    {
        private readonly string secretKey;
        private readonly IConfiguration _config; // agregado
        public readonly ContextDb  _context;

        public AutentificacionController(IConfiguration config, ContextDb context)
        {
            _config = config; // asignado el valor de config
            secretKey = _config.GetSection("Settings").GetValue<string>("secret");
            _context = context;
        }

        [HttpPost]
        [Route("validar")]
        public async Task<IActionResult> Validar([FromBody] AutorizacionDTO request)
        {
            if (request == null)
                return BadRequest("Datos inválidos");

          

            
                var user = await _context.Personas.FirstOrDefaultAsync(u => u.Dni == request.Dni && u.Contraseña == request.Contrasenia);

                if (user != null)
                {
                    var token = GenerarToken(request.Dni, request.Contrasenia);
                return Ok(new
                {
                    token = token,
                    FechaExpiracion = DateTime.UtcNow.AddMinutes(60),
                    usuario = new
                    {
                        Id = user.Id,
                        Foto = user.Foto,
                        Dni = user.Dni,
                        Nombre = user.Nombre,
                        Apellido = user.Apellido,
                        FechaNacimiento = user.FechaNacimiento,
                        Calle = user.Calle,
                        numero = user.Numero,
                        ciudad = user.Ciudad,
                        nTelefono1 = user.NTelefono1,
                        EsJugador = user.EsJugador,
                        EsDirectorTecnico = user.EsDirectorTecnico,
                        EsRepresentanteEquipo = user.EsRepresentanteEquipo,
                        EsRepresentanteAsociacion = user.EsEncargadoAsociacion,
                        nSocio = user.NSocio,
                        IdCategoria = user.IdCategoria,
                        IdDivision = user.IdDivision
                    }

                });
                }
                else
                {
                    return BadRequest("Credenciales inválidas.");
                }
            
        }

        private string GenerarToken(string dni, string contrasenia)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, dni),
                new Claim(ClaimTypes.NameIdentifier, dni)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddMinutes(60), signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}