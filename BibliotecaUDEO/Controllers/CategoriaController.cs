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
    public class CategoriaController : ControllerBase
    {
        private readonly BibliotecaUDEOContext _context;

        public static IWebHostEnvironment _environment;

        public CategoriaController(BibliotecaUDEOContext context, IWebHostEnvironment environment)
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
            int totalCount;
            List<Categorium> categorias = new List<Categorium>();

            if (filterByName != null)
            {
                decimal total_records = await _context.Categoria.Where(x => x.Nombre.Contains(filterByName)).CountAsync();
                totalCount = Convert.ToInt32(total_records);
                total_page = Convert.ToInt32(Math.Ceiling(total_records / _records));
                categorias = await _context.Categoria.Where(x => x.Nombre.Contains(filterByName)).Skip((_page - 1) * _records).Take(_records).ToListAsync();
            }
            else
            {
                decimal total_records = await _context.Categoria.CountAsync();
                totalCount = Convert.ToInt32(total_records);
                total_page = Convert.ToInt32(Math.Ceiling(total_records / _records));
                categorias = await _context.Categoria.Skip((_page - 1) * _records).Take(_records).ToListAsync();
            }

            return Ok(new
            {
                totalCount = totalCount,
                records = categorias,
                current_page = _page
            });
        }

        // GET: api/Categoria/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Categorium>> GetCategorium(int id)
        {
            var categorium = await _context.Categoria.FindAsync(id);

            if (categorium == null)
            {
                return NotFound();
            }

            return categorium;
        }

        // GET: api/Categoria/SearchByName/{SearchString: string}
        [Authorize]
        [HttpGet("SearchByName/{SearchString}")]
        public async Task<ActionResult<IEnumerable<Categorium>>> GetCategoriaByName(string SearchString)
        {
            var item = from m in _context.Categoria
                       select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                item = item.Where(s => s.Nombre.Contains(SearchString));
            }

            return await item.ToListAsync();
        }

        // PUT: api/Categoria/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategorium(int id, Categorium categorium)
        {
            if (id != categorium.Id)
            {
                return BadRequest();
            }

            _context.Entry(categorium).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriumExists(id))
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

        // POST: api/Categoria
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Categorium>> PostCategorium(Categorium categorium)
        {
            _context.Categoria.Add(categorium);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategorium", new { id = categorium.Id }, categorium);
        }

        // DELETE: api/Categoria/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategorium(int id)
        {
            var categorium = await _context.Categoria.FindAsync(id);
            if (categorium == null)
            {
                return NotFound();
            }

            _context.Categoria.Remove(categorium);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoriumExists(int id)
        {
            return _context.Categoria.Any(e => e.Id == id);
        }
    }
}
