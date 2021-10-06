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
    public class AutorController : ControllerBase
    {
        private readonly BibliotecaUDEOContext _context;

        public static IWebHostEnvironment _environment;

        public AutorController(BibliotecaUDEOContext context, IWebHostEnvironment environment)
        {
            _context = context;

            _environment = environment;
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] string filterByName, int? page, int? records)
        {
            int _page = page ?? 1;
            int _records = records ?? 7;
            int total_page;
            int totalCount;
            List<Autor> autores = new List<Autor>();

            if (filterByName != null)
            {
                decimal total_records = await _context.Autors.Where(x => x.Nombre.Contains(filterByName)).CountAsync();
                totalCount = Convert.ToInt32(total_records);
                total_page = Convert.ToInt32(Math.Ceiling(total_records / _records));
                autores = await _context.Autors.Where(x => x.Nombre.Contains(filterByName)).Skip((_page - 1) * _records).Take(_records).ToListAsync();
            }
            else
            {
                decimal total_records = await _context.Autors.CountAsync();
                totalCount = Convert.ToInt32(total_records);
                total_page = Convert.ToInt32(Math.Ceiling(total_records / _records));
                autores = await _context.Autors.Skip((_page - 1) * _records).Take(_records).ToListAsync();
            }

            return Ok(new
            {
                totalCount = totalCount,
                records = autores,
                current_page = _page
            });
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

        // GET: api/Autor/SearchByName/{SearchString: string}
        [Authorize]
        [HttpGet("SearchByName/{SearchString}")]
        public async Task<ActionResult<IEnumerable<Autor>>> GetAutorByName(string SearchString)
        {
            var item = from m in _context.Autors
                        select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                item = item.Where(s => s.Nombre.Contains(SearchString));
            }

            return await item.ToListAsync();
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
