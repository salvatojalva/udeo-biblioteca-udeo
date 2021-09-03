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
    public class EditorialController : ControllerBase
    {
        private readonly BibliotecaUDEOContext _context;

        public EditorialController(BibliotecaUDEOContext context)
        {
            _context = context;
        }

        // GET: api/Editorial
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Editorial>>> GetEditorials()
        {
            return await _context.Editorials.ToListAsync();
        }

        // GET: api/Editorial/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Editorial>> GetEditorial(int id)
        {
            var editorial = await _context.Editorials.FindAsync(id);

            if (editorial == null)
            {
                return NotFound();
            }

            return editorial;
        }

        // GET: api/Editorial/SearchByName/{SearchString: string}
        [Authorize]
        [HttpGet("SearchByName/{SearchString}")]
        public async Task<ActionResult<IEnumerable<Editorial>>> GetByName(string SearchString)
        {
            var item = from m in _context.Editorials
                       select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                item = item.Where(s => s.Nombre.Contains(SearchString));
            }

            return await item.ToListAsync();
        }

        // PUT: api/Editorial/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEditorial(int id, Editorial editorial)
        {
            if (id != editorial.Id)
            {
                return BadRequest();
            }

            _context.Entry(editorial).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EditorialExists(id))
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

        // POST: api/Editorial
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Editorial>> PostEditorial(Editorial editorial)
        {
            _context.Editorials.Add(editorial);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EditorialExists(editorial.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEditorial", new { id = editorial.Id }, editorial);
        }

        // DELETE: api/Editorial/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEditorial(int id)
        {
            var editorial = await _context.Editorials.FindAsync(id);
            if (editorial == null)
            {
                return NotFound();
            }

            _context.Editorials.Remove(editorial);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EditorialExists(int id)
        {
            return _context.Editorials.Any(e => e.Id == id);
        }
    }
}
