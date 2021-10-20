using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaUDEO.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace BibliotecaUDEO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly BibliotecaUDEOContext _context;
        public static IWebHostEnvironment _environment;

        public UsuarioController(BibliotecaUDEOContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        [HttpPost("StoreUsuario")]
        public async Task<ActionResult<Usuario>> PostUserFormData([FromForm] UsuarioFormData userFormData)
        {
            string endpointimagen;

            endpointimagen = "";

            if (userFormData.archivo.Length > 0)
            {

                if (!Directory.Exists(_environment.WebRootPath + "\\Uplods\\"))
                {
                    Directory.CreateDirectory(_environment.WebRootPath + "\\Uplods\\");
                }

                DateTime foo = DateTime.Now;
                long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();

                string[] formatosadmitidos = { ".PNG", ".JPG", ".PDF" };

                string FormatoArchivo = Path.GetExtension(userFormData.archivo.FileName).ToUpper();

                if (formatosadmitidos.Contains(FormatoArchivo))
                {

                    string NombreArchivo = userFormData.archivo.FileName;
                    NombreArchivo = Convert.ToString(unixTime) + FormatoArchivo;

                    var filpath = _environment.WebRootPath + "\\Uplods\\" + NombreArchivo;

                    using (FileStream fileStream = System.IO.File.Create(filpath))
                    {

                        userFormData.archivo.CopyTo(fileStream);
                        fileStream.Flush();

                        endpointimagen =  NombreArchivo;

                    }

                }

            }

            Usuario user = new Usuario();

            user.Nombre = userFormData.nombre;
            user.Apellido = userFormData.apellido;
            user.GoogleId = userFormData.googleId;
            user.Activo = true;
            user.Rol = userFormData.rol;
            user.Image = endpointimagen;

            _context.Usuarios.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = user.Id }, user);
        }

        // GET: api/Usuario

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery]  string filterByName, int? page, int? records)
        {
            int _page = page ?? 1;
            int _records = records ?? 10;
            int total_page;
            List<Usuario> usuarios = new List<Usuario>();

            if (filterByName != null)
            {
                decimal total_records = await _context.Usuarios.Where(x => x.Nombre.Contains(filterByName)).CountAsync();
                total_page = Convert.ToInt32(Math.Ceiling(total_records / _records));
                usuarios = await _context.Usuarios.Where(x => x.Nombre.Contains(filterByName)).Skip((_page - 1) * _records).Take(_records).ToListAsync();
            }
            else
            {
                decimal total_records = await _context.Usuarios.CountAsync();
                total_page = Convert.ToInt32(Math.Ceiling(total_records / _records));
                usuarios = await _context.Usuarios.Skip((_page - 1) * _records).Take(_records).ToListAsync();
            }

            return Ok(new
            {
                pages = total_page,
                records = usuarios,
                current_page = _page
            });
        }
        

        // GET: api/Usuario/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // PUT: api/Usuario/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Usuario
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
        }

        // DELETE: api/Usuario/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }

    public class UsuarioFormData
    {
        public IFormFile archivo { get; set; }
        public String nombre { get; set; }
        public String apellido { get; set; }
        public String googleId { get; set; }
        public String rol { get; set; }
 
    }
}
