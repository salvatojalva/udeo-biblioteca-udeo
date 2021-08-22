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
    public class AutorController : ControllerBase
    {
        private readonly BibliotecaUDEOContext _context;

        public AutorController(BibliotecaUDEOContext context)
        {
            _context = context;
        }

        // GET: api/Autor
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Autor>>> GetAutors()
        {
            return await _context.Autors.ToListAsync();
        }

        // GET: api/Autor/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Autor>> GetAutor(int id)
        {
            var autor = await _context.Autors.FindAsync(id);

            if (autor == null)
            {
                return NotFound();
            }

            return autor;
        }

        // PUT: api/Autor/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAutor(int id, Autor autor)
        {
            if (id != autor.Id)
            {
                return BadRequest();
            }

            _context.Entry(autor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AutorExists(id))
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

        // POST: api/Autor
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Autor>> PostAutor(Autor autor)
        {
            _context.Autors.Add(autor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAutor", new { id = autor.Id }, autor);
        }

        // DELETE: api/Autor/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAutor(int id)
        {
            var autor = await _context.Autors.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }

            _context.Autors.Remove(autor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AutorExists(int id)
        {
            return _context.Autors.Any(e => e.Id == id);
        }
    }
}
