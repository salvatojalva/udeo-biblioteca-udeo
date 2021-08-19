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
    public class DocumentoItemController : ControllerBase
    {
        private readonly BibliotecaUDEOContext _context;

        public DocumentoItemController(BibliotecaUDEOContext context)
        {
            _context = context;
        }

        // GET: api/DocumentoItem
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentoItem>>> GetDocumentoItems()
        {
            return await _context.DocumentoItems.ToListAsync();
        }

        // GET: api/DocumentoItem/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DocumentoItem>> GetDocumentoItem(int id)
        {
            var documentoItem = await _context.DocumentoItems.FindAsync(id);

            if (documentoItem == null)
            {
                return NotFound();
            }

            return documentoItem;
        }

        // PUT: api/DocumentoItem/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocumentoItem(int id, DocumentoItem documentoItem)
        {
            if (id != documentoItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(documentoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentoItemExists(id))
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

        // POST: api/DocumentoItem
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DocumentoItem>> PostDocumentoItem(DocumentoItem documentoItem)
        {
            _context.DocumentoItems.Add(documentoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDocumentoItem", new { id = documentoItem.Id }, documentoItem);
        }

        // DELETE: api/DocumentoItem/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocumentoItem(int id)
        {
            var documentoItem = await _context.DocumentoItems.FindAsync(id);
            if (documentoItem == null)
            {
                return NotFound();
            }

            _context.DocumentoItems.Remove(documentoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocumentoItemExists(int id)
        {
            return _context.DocumentoItems.Any(e => e.Id == id);
        }
    }
}
