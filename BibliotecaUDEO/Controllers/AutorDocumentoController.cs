using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaUDEO.Models;

namespace BibliotecaUDEO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorDocumentoController : ControllerBase
    {
        private readonly BibliotecaUDEOContext _context;

        public AutorDocumentoController(BibliotecaUDEOContext context)
        {
            _context = context;
        }

        // GET: api/AutorDocumento
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AutorDocumento>>> GetAutorDocumentos()
        {
            return await _context.AutorDocumentos.ToListAsync();
        }

        // GET: api/AutorDocumento/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AutorDocumento>> GetAutorDocumento(int id)
        {
            var autorDocumento = await _context.AutorDocumentos.FindAsync(id);

            if (autorDocumento == null)
            {
                return NotFound();
            }

            return autorDocumento;
        }

        // PUT: api/AutorDocumento/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAutorDocumento(int id, AutorDocumento autorDocumento)
        {
            if (id != autorDocumento.Id)
            {
                return BadRequest();
            }

            _context.Entry(autorDocumento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AutorDocumentoExists(id))
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

        // POST: api/AutorDocumento
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AutorDocumento>> PostAutorDocumento(AutorDocumento autorDocumento)
        {
            _context.AutorDocumentos.Add(autorDocumento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAutorDocumento", new { id = autorDocumento.Id }, autorDocumento);
        }

        // DELETE: api/AutorDocumento/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAutorDocumento(int id)
        {
            var autorDocumento = await _context.AutorDocumentos.FindAsync(id);
            if (autorDocumento == null)
            {
                return NotFound();
            }

            _context.AutorDocumentos.Remove(autorDocumento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AutorDocumentoExists(int id)
        {
            return _context.AutorDocumentos.Any(e => e.Id == id);
        }
    }
}
