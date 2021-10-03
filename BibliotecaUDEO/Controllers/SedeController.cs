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
    public class SedeController : ControllerBase
    {
        private readonly BibliotecaUDEOContext _context;

        public SedeController(BibliotecaUDEOContext context)
        {
            _context = context;
        }

        // GET: api/Sede
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] string filterByName, int? page, int? records)
        {
            int _page = page ?? 1;
            int _records = records ?? 10;
            int total_page;
            List<Sede> sede = new List<Sede>();

            if (filterByName != null)
            {
                decimal total_records = await _context.Sedes.Where(x => x.Nombre.Contains(filterByName)).CountAsync();
                total_page = Convert.ToInt32(Math.Ceiling(total_records / _records));
               sede = await _context.Sedes.Where(x => x.Nombre.Contains(filterByName)).Skip((_page - 1) * _records).Take(_records).ToListAsync();
            }
            else
            {
                decimal total_records = await _context.Sedes.CountAsync();
                total_page = Convert.ToInt32(Math.Ceiling(total_records / _records));
                sede = await _context.Sedes.Skip((_page - 1) * _records).Take(_records).ToListAsync();
            }
            return Ok(new
            {
                pages = total_page,
                records = sede,
                current_page = _page
            });
        }

        // GET: api/Sede/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Sede>> GetSede(int id)
        {
            var sede = await _context.Sedes.FindAsync(id);

            if (sede == null)
            {
                return NotFound();
            }

            return sede;
        }

        // GET: api/Sede/SearchByName/{SearchString: string}
        [Authorize]
        [HttpGet("SearchByName/{SearchString}")]
        public async Task<ActionResult<IEnumerable<Sede>>> GetByName(string SearchString)
        {
            var item = from m in _context.Sedes
                       select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                item = item.Where(s => s.Nombre.Contains(SearchString));
            }

            return await item.ToListAsync();
        }

        // PUT: api/Sede/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSede(int id, Sede sede)
        {
            if (id != sede.Id)
            {
                return BadRequest();
            }

            _context.Entry(sede).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SedeExists(id))
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

        // POST: api/Sede
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Sede>> PostSede(Sede sede)
        {
            _context.Sedes.Add(sede);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSede", new { id = sede.Id }, sede);
        }

        // DELETE: api/Sede/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSede(int id)
        {
            var sede = await _context.Sedes.FindAsync(id);
            if (sede == null)
            {
                return NotFound();
            }

            _context.Sedes.Remove(sede);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SedeExists(int id)
        {
            return _context.Sedes.Any(e => e.Id == id);
        }
    }
}
