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
            secretKey = _config.GetSection("Settings").GetSection("secret").ToString();
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
                    return Ok(new { token = token });//return Ok(token);
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

            var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddMinutes(30), signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}