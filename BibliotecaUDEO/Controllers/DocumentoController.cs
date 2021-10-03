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
    public class DocumentoController : ControllerBase
    {
        private readonly BibliotecaUDEOContext _context;

        public DocumentoController(BibliotecaUDEOContext context)
        {
            _context = context;
        }

        // GET: api/Documento
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] string filterByTitle, int? page, int? records)
        {
            int _page = page ?? 1;
            int _records = records ?? 10;
            int total_page;
            List<Documento> documento = new List<Documento>();
            if (filterByTitle != null)
            {
                decimal total_records = await _context.Documentos.Where(x => x.Titulo.Contains(filterByTitle)).CountAsync();
                total_page = Convert.ToInt32(Math.Ceiling(total_records / _records));
                documento = await _context.Documentos.Where(x => x.Titulo.Contains(filterByTitle)).Skip((_page - 1) * _records).Take(_records).ToListAsync();
            }
            else
            {
                decimal total_records = await _context.Documentos.CountAsync();
                total_page = Convert.ToInt32(Math.Ceiling(total_records / _records));
                documento = await _context.Documentos.Skip((_page - 1) * _records).Take(_records).ToListAsync();
            }
            return Ok(new
            {
                pages = total_page,
                records = documento,
                current_page = _page
            });


        }

        // GET: api/Documento/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Documento>> GetDocumento(int id)
        {
            var documento = await _context.Documentos.FindAsync(id);

            if (documento == null)
            {
                return NotFound();
            }

            return documento;
        }

        // PUT: api/Documento/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocumento(int id, Documento documento)
        {
            if (id != documento.Id)
            {
                return BadRequest();
            }

            _context.Entry(documento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentoExists(id))
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

        // POST: api/Documento
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Documento>> PostDocumento(Documento documento)
        {
            _context.Documentos.Add(documento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDocumento", new { id = documento.Id }, documento);
        }

        // DELETE: api/Documento/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocumento(int id)
        {
            var documento = await _context.Documentos.FindAsync(id);
            if (documento == null)
            {
                return NotFound();
            }

            _context.Documentos.Remove(documento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocumentoExists(int id)
        {
            return _context.Documentos.Any(e => e.Id == id);
        }
    }
}
