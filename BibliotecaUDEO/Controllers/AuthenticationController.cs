using AuthService;
using BibliotecaUDEO.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaUDEO.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly BibliotecaUDEOContext _context;
        public class AuthenticateRequest
        {
            [Required]
            public string IdToken { get; set; }
        }

        private readonly JwtGenerator _jwtGenerator;

        public AuthenticationController(IConfiguration configuration, BibliotecaUDEOContext context)
        {
            _context = context;
            _jwtGenerator = new JwtGenerator(configuration.GetValue<string>("JwtPrivateSigningKey"));
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest data)
        {
            GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings();

            Usuario usuario = new Usuario();

            // Change this to your google client ID
            settings.Audience = new List<string>() { "63471605255-aq235m5sfiiio2jplto5i3gmp0kermao.apps.googleusercontent.com" };

            GoogleJsonWebSignature.Payload payload = GoogleJsonWebSignature.ValidateAsync(data.IdToken, settings).Result;

            //if(payload.HostedDomain != "udeo.edu.gt")
                //return Unauthorized(new { Ok = false,  Code = 403, Message = "Tu correo no pertenese a la organizacion"});

            var find_usuario = await  _context.Usuarios.Where(x => x.GoogleId == payload.Email).FirstOrDefaultAsync();

            if (find_usuario == null)
            {
                usuario.Nombre = payload.Name;
                usuario.Apellido = payload.FamilyName;
                usuario.GoogleId = payload.Email;
                usuario.Rol = "student";

                _context.Usuarios.Add(usuario);

                await _context.SaveChangesAsync();

            }
            else {
                usuario = find_usuario;
            }

            return Ok(new {
                AuthToken = _jwtGenerator.CreateUserAuthToken(payload.Email),
                expiresIn = 120*60,
                user = usuario
            });
        }
    }
}