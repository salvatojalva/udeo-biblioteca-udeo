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
    public class CarreraController : ControllerBase
    {
        private readonly BibliotecaUDEOContext _context;

        public static IWebHostEnvironment _environment;

        public CarreraController(BibliotecaUDEOContext context, IWebHostEnvironment environment)
        {
            _context = context;

            _environment = environment;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] string filterByName, int? page, int? records)
        {
            int _page = page ?? 1;
            int _records = records ?? 2;
            int total_page;
            int totalCount;
            List<Carrera> carreras = new List<Carrera>();

            if (filterByName != null)
            {
                decimal total_records = await _context.Carreras.Where(x => x.Nombre.Contains(filterByName)).CountAsync();
                totalCount = Convert.ToInt32(total_records);
                total_page = Convert.ToInt32(Math.Ceiling(total_records / _records));
                carreras = await _context.Carreras.Where(x => x.Nombre.Contains(filterByName)).Skip((_page - 1) * _records).Take(_records).ToListAsync();
            }
            else
            {
                decimal total_records = await _context.Carreras.CountAsync();
                totalCount = Convert.ToInt32(total_records);
                total_page = Convert.ToInt32(Math.Ceiling(total_records / _records));
                carreras = await _context.Carreras.Skip((_page - 1) * _records).Take(_records).ToListAsync();
            }

            return Ok(new
            {
                totalCount = totalCount,
                records = carreras,
                current_page = _page
            });
        }

        // GET: api/Carrera/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Carrera>> GetCarrera(int id)
        {
            var carrera = await _context.Carreras.FindAsync(id);

            if (carrera == null)
            {
                return NotFound();
            }

            return carrera;
        }

        // GET: api/Carrera/SearchByName/{SearchString: string}
        [Authorize]
        [HttpGet("SearchByName/{SearchString}")]
        public async Task<ActionResult<IEnumerable<Carrera>>> GetCarreraByName(string SearchString)
        {
            var item = from m in _context.Carreras
                       select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                item = item.Where(s => s.Nombre.Contains(SearchString));
            }

            return await item.ToListAsync();
        }

        // PUT: api/Carrera/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCarrera(int id, Carrera carrera)
        {
            if (id != carrera.Id)
            {
                return BadRequest();
            }

            _context.Entry(carrera).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarreraExists(id))
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

        // POST: api/Carrera
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Carrera>> PostCarrera(Carrera carrera)
        {
            _context.Carreras.Add(carrera);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCarrera", new { id = carrera.Id }, carrera);
        }

        // DELETE: api/Carrera/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarrera(int id)
        {
            var carrera = await _context.Carreras.FindAsync(id);
            if (carrera == null)
            {
                return NotFound();
            }

            _context.Carreras.Remove(carrera);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CarreraExists(int id)
        {
            return _context.Carreras.Any(e => e.Id == id);
        }
    }
}
