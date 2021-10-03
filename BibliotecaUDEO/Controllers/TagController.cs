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
    public class TagController : ControllerBase
    {
        private readonly BibliotecaUDEOContext _context;

        public TagController(BibliotecaUDEOContext context)
        {
            _context = context;
        }

        // GET: api/Tag
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] string filterByName, int? page, int? records) 
        {
            int _page = page ?? 1;
            int _records = records ?? 10;
            int total_page;
            List<Tag> tag = new List<Tag>();

            if (filterByName!=null)
            {
                decimal total_records = await _context.Tags.Where(x => x.Nombre.Contains(filterByName)).CountAsync();
                total_page = Convert.ToInt32(Math.Ceiling(total_records / _records));
                tag = await _context.Tags.Where(x => x.Nombre.Contains(filterByName)).Skip((_page - 1) * _records).Take(_records).ToListAsync();

            }
            else
            {
                decimal total_records = await _context.Tags.CountAsync();
                total_page = Convert.ToInt32(Math.Ceiling(total_records / _records));
                tag = await _context.Tags.Skip((_page - 1) * _records).Take(_records).ToListAsync();
            }

            return Ok(new
            {
                pages = total_page,
                records = tag,
                current_page = _page
            });

        }

        // GET: api/Tag/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Tag>> GetTag(int id)
        {
            var tag = await _context.Tags.FindAsync(id);

            if (tag == null)
            {
                return NotFound();
            }

            return tag;
        }

        // GET: api/Tag/SearchByName/{SearchString: string}
        [Authorize]
        [HttpGet("SearchByName/{SearchString}")]
        public async Task<ActionResult<IEnumerable<Tag>>> GetByName(string SearchString)
        {
            var item = from m in _context.Tags
                       select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                item = item.Where(s => s.Nombre.Contains(SearchString));
            }

            return await item.ToListAsync();
        }

        // PUT: api/Tag/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTag(int id, Tag tag)
        {
            if (id != tag.Id)
            {
                return BadRequest();
            }

            _context.Entry(tag).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TagExists(id))
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

        // POST: api/Tag
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Tag>> PostTag(Tag tag)
        {
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTag", new { id = tag.Id }, tag);
        }

        // DELETE: api/Tag/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TagExists(int id)
        {
            return _context.Tags.Any(e => e.Id == id);
        }
    }
}
