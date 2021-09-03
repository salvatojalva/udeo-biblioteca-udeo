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
    public class DivisionController : ControllerBase
    {
        private readonly BibliotecaUDEOContext _context;

        public DivisionController(BibliotecaUDEOContext context)
        {
            _context = context;
        }

        // GET: api/Division
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Division>>> GetDivisions()
        {
            return await _context.Divisions.ToListAsync();
        }

        // GET: api/Division/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Division>> GetDivision(int id)
        {
            var division = await _context.Divisions.FindAsync(id);

            if (division == null)
            {
                return NotFound();
            }

            return division;
        }

        // GET: api/Division/SearchByName/{SearchString: string}
        [Authorize]
        [HttpGet("SearchByName/{SearchString}")]
        public async Task<ActionResult<IEnumerable<Division>>> GetDivisionByName(string SearchString)
        {
            var item = from m in _context.Divisions
                       select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                item = item.Where(s => s.Nombre.Contains(SearchString));
            }

            return await item.ToListAsync();
        }

        // PUT: api/Division/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDivision(int id, Division division)
        {
            if (id != division.Id)
            {
                return BadRequest();
            }

            _context.Entry(division).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DivisionExists(id))
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

        // POST: api/Division
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Division>> PostDivision(Division division)
        {
            _context.Divisions.Add(division);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDivision", new { id = division.Id }, division);
        }

        // DELETE: api/Division/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDivision(int id)
        {
            var division = await _context.Divisions.FindAsync(id);
            if (division == null)
            {
                return NotFound();
            }

            _context.Divisions.Remove(division);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DivisionExists(int id)
        {
            return _context.Divisions.Any(e => e.Id == id);
        }
    }
}
