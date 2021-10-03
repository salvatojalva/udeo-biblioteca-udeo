using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaUDEO.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace BibliotecaUDEO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnioController : ControllerBase
    {
        private readonly BibliotecaUDEOContext _context;

        public static IWebHostEnvironment _environment;

        public AnioController(BibliotecaUDEOContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] string filterByName, int? page, int? records)
        {
            int _page = page ?? 1;
            int _records = records ?? 2;
            int total_page;
            List<Anio> anio = new List<Anio>();

            if (filterByName != null)
            {
                decimal total_records = await _context.Anios.Where(x => x.Nombre.Contains(filterByName)).CountAsync();
                total_page = Convert.ToInt32(Math.Ceiling(total_records / _records));
                anio = await _context.Anios.Where(x => x.Nombre.Contains(filterByName)).Skip((_page - 1) * _records).Take(_records).ToListAsync();
            }
            else
            {
                decimal total_records = await _context.Anios.CountAsync();
                total_page = Convert.ToInt32(Math.Ceiling(total_records / _records));
                anio = await _context.Anios.Skip((_page - 1) * _records).Take(_records).ToListAsync();
            }

            return Ok(new
            {
                pages = total_page,
                records = anio,
                current_page = _page
            });
        }


        // GET: api/Anio/5
        [Authorize]
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

        // GET: api/Anio/SearchByName/{SearchString: string}
        [Authorize]
        [HttpGet("SearchByName/{SearchString}")]
        public async Task<ActionResult<IEnumerable<Anio>>> GetAnioByName(string SearchString)
        {
            var anios = from m in _context.Anios
                         select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                anios = anios.Where(s => s.Nombre.Contains(SearchString));
            }

            return await anios.ToListAsync();
        }


        // PUT: api/Anio/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
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
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Anio>> PostAnio(Anio anio)
        {
            _context.Anios.Add(anio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnio", new { id = anio.Id }, anio);
        }

        // DELETE: api/Anio/5
        [Authorize]
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

    public class AnioFormData
    {
        public String nombre { get; set; }
    }
}
