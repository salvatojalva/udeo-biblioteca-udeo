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
    public class AnioController : ControllerBase
    {
        private readonly BibliotecaUDEOContext _context;

        public AnioController(BibliotecaUDEOContext context)
        {
            _context = context;
        }

        // GET: api/Anio
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Anio>>> GetAnios()
        {
            return await _context.Anios.ToListAsync();
        }

        // GET: api/Anio/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Anio>> GetAnio(int id)
        {
            var anio = await _context.Anios.FindAsync(id);

            if (anio == null)
            {
                return NotFound();
            }

            return anio;
        }

        // PUT: api/Anio/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnio(int id, Anio anio)
        {
            if (id != anio.Id)
            {
                return BadRequest();
            }

            _context.Entry(anio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnioExists(id))
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

        // POST: api/Anio
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Anio>> PostAnio(Anio anio)
        {
            _context.Anios.Add(anio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnio", new { id = anio.Id }, anio);
        }

        // DELETE: api/Anio/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnio(int id)
        {
            var anio = await _context.Anios.FindAsync(id);
            if (anio == null)
            {
                return NotFound();
            }

            _context.Anios.Remove(anio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AnioExists(int id)
        {
            return _context.Anios.Any(e => e.Id == id);
        }
    }
}
