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
        public async Task<ActionResult> Get([FromQuery] string filterByName, int? page, int? records) 
        {
            int _page = page ?? 1;
            int _records = records ?? 7;
            int total_page;
            int totalCount;
            List<Division> division = new List<Division>();

            if (filterByName != null)
            {
                decimal total_records = await _context.Divisions.Where(x => x.Nombre.Contains(filterByName)).CountAsync();
                totalCount = Convert.ToInt32(total_records);
                total_page = Convert.ToInt32(Math.Ceiling(total_records / _records));
                division= await _context.Divisions.Where(x => x.Nombre.Contains(filterByName)).Skip((_page - 1) * _records).Take(_records).ToListAsync();
            }
            else
            {
                decimal total_records = await _context.Divisions.CountAsync();
                totalCount = Convert.ToInt32(total_records);
                total_page = Convert.ToInt32(Math.Ceiling(total_records / _records));
                division = await _context.Divisions.Skip((_page - 1) * _records).Take(_records).ToListAsync();
            }
            return Ok(new
            {
                totalCount = totalCount,
                records = division,
                current_page = _page
            });
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
