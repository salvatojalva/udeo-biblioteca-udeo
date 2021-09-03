using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaUDEO.Models;
using Microsoft.AspNetCore.Authorization;

namespace BibliotecaUDEO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoDocumentoController : ControllerBase
    {
        private readonly BibliotecaUDEOContext _context;

        public TipoDocumentoController(BibliotecaUDEOContext context)
        {
            _context = context;
        }

        // GET: api/TipoDocumento
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoDocumento>>> GetTipoDocumentos()
        {
            return await _context.TipoDocumentos.ToListAsync();
        }

        // GET: api/TipoDocumento/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoDocumento>> GetTipoDocumento(int id)
        {
            var tipoDocumento = await _context.TipoDocumentos.FindAsync(id);

            if (tipoDocumento == null)
            {
                return NotFound();
            }

            return tipoDocumento;
        }

        // PUT: api/TipoDocumento/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoDocumento(int id, TipoDocumento tipoDocumento)
        {
            if (id != tipoDocumento.Id)
            {
                return BadRequest();
            }

            _context.Entry(tipoDocumento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoDocumentoExists(id))
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

        // POST: api/TipoDocumento
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<TipoDocumento>> PostTipoDocumento(TipoDocumento tipoDocumento)
        {
            _context.TipoDocumentos.Add(tipoDocumento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoDocumento", new { id = tipoDocumento.Id }, tipoDocumento);
        }

        // DELETE: api/TipoDocumento/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoDocumento(int id)
        {
            var tipoDocumento = await _context.TipoDocumentos.FindAsync(id);
            if (tipoDocumento == null)
            {
                return NotFound();
            }

            _context.TipoDocumentos.Remove(tipoDocumento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipoDocumentoExists(int id)
        {
            return _context.TipoDocumentos.Any(e => e.Id == id);
        }
    }
}
