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
    public class PrestamoController : ControllerBase
    {
        private readonly BibliotecaUDEOContext _context;

        public PrestamoController(BibliotecaUDEOContext context)
        {
            _context = context;
        }

        // GET: api/Prestamo
        [Authorize]
        [HttpGet("Devuelto")]
        public async Task<ActionResult<Prestamo>> GetPrestamos([FromQuery]int UsuarioID, int filterbydevuelto, int? page, int? records)
        {
            int _page = page ?? 1;
            int _records = records ?? 1;
            int total_page;
            int totalCount;
            List<Prestamo> prestamo = new List<Prestamo>();

            if (filterbydevuelto==0||filterbydevuelto==1)
            {
                decimal total_records = await _context.Prestamos
                    .Where(x => x.UsuarioId == UsuarioID&&x.Devuelto==Convert.ToBoolean(filterbydevuelto))
                    .CountAsync();
                totalCount = Convert.ToInt32(total_records);
                total_page = Convert.ToInt32(Math.Ceiling(total_records / _records));
                prestamo = await _context.Prestamos
                    .Include(documentoitem => documentoitem.DocumentoItem)
                             .ThenInclude(documento => documento.Documento)
                    .Where(x => x.UsuarioId == UsuarioID && x.Devuelto == Convert.ToBoolean(filterbydevuelto))
                    .Skip((_page - 1) * _records).Take(_records).ToListAsync();
            }
            else
            {
                decimal total_records = await _context.Prestamos
                    .Where(usuario=>usuario.UsuarioId==UsuarioID)
                    .CountAsync();
                totalCount = Convert.ToInt32(total_records);
                total_page = Convert.ToInt32(Math.Ceiling(total_records / _records));
                prestamo = await _context.Prestamos
                     .Include(documentoitem => documentoitem.DocumentoItem)
                             .ThenInclude(documento => documento.Documento)
                             .Where(usuario => usuario.UsuarioId == UsuarioID)
                    .Skip((_page - 1) * _records).Take(_records).ToListAsync();
            }

            return Ok(new
            {
                current_page = _page,
                totalCount = totalCount,
                resul = prestamo
            });
        }

        [Authorize]
        [HttpGet("FechaFin")]
        public async Task<ActionResult<Prestamo>> Get([FromQuery] int UsuarioID, bool filterbydevuelto, int? page, int? records)
        {
            int _page = page ?? 1;
            int _records = records ?? 1;
            int total_page;
            int totalCount;
            List<Prestamo> prestamo = new List<Prestamo>();

                decimal total_records = await _context.Prestamos
                    .Where(usuario => usuario.UsuarioId == UsuarioID && usuario.FechaFin <= DateTime.Now)
                    .CountAsync();
                totalCount = Convert.ToInt32(total_records);
                total_page = Convert.ToInt32(Math.Ceiling(total_records / _records));
            prestamo = await _context.Prestamos
                 .Include(documentoitem => documentoitem.DocumentoItem)
                         .ThenInclude(documento => documento.Documento)
                         .Where(usuario => usuario.UsuarioId == UsuarioID&&usuario.FechaFin<=DateTime.Now)
                    .Skip((_page - 1) * _records).Take(_records).ToListAsync();


            return Ok(new
            {
                current_page = _page,
                totalCount = totalCount,
                resul = prestamo
            });
        }

        [Authorize]
        [HttpGet("Todo")]
        public async Task<ActionResult<Prestamo>> Get([FromQuery] int? page, int? records)
        {
            int _page = page ?? 1;
            int _records = records ?? 1;
            int total_page;
            int totalCount;
            List<Prestamo> prestamo = new List<Prestamo>();

            decimal total_records = await _context.Prestamos
                .Include(documentoitem => documentoitem.DocumentoItem)
                .Include(usuario=>usuario.Usuario)
                .CountAsync();              
            totalCount = Convert.ToInt32(total_records);
            total_page = Convert.ToInt32(Math.Ceiling(total_records / _records));
            prestamo = await _context.Prestamos
                 .Include(documentoitem => documentoitem.DocumentoItem)
                .Include(usuario => usuario.Usuario)
                .Where(x => x.UsuarioId == 1).Skip((_page - 1) * _records)
                .Take(_records).ToListAsync();

            return Ok(new
            {
                current_page = _page,
                totalCount = totalCount,
                resul = prestamo
            });
        }

        // GET: api/Prestamo/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Prestamo>> GetPrestamo(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);

            if (prestamo == null)
            {
                return NotFound();
            }

            return prestamo;
        }

        // PUT: api/Prestamo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrestamo(int id, Prestamo prestamo)
        {
            if (id != prestamo.Id)
            {
                return BadRequest();
            }

            _context.Entry(prestamo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrestamoExists(id))
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

        // POST: api/Prestamo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Prestamo>> PostPrestamo([FromForm]Prestamo prestamo)
        {
            prestamo.Aprobado = true;
            prestamo.Denegado = false;

            prestamo.FechaFin = new DateTime().AddDays(7);
            prestamo.FechaInicio = new DateTime();

            var documentoItem = await _context.DocumentoItems.FindAsync(prestamo.DocumentoItemId);

            if (documentoItem.EsFisico == true)
            {
                documentoItem.Activo = false;
            }
            documentoItem.NumeroPrestamos++;



            _context.Prestamos.Add(prestamo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPrestamo", new { id = prestamo.Id }, prestamo);
        }

        // POST: api/Prestamo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost("Solicitar")]
        public async Task<ActionResult<Prestamo>> SolicitarPrestamo([FromForm] Prestamo prestamo)
        {
            prestamo.Aprobado = false;
            prestamo.Denegado = false;

            _context.Prestamos.Add(prestamo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPrestamo", new { id = prestamo.Id }, prestamo);
        }



        [Authorize]
        [HttpPost("Aprobar")]

        public async Task<IActionResult> AprobarPrestamo(int id, Prestamo prestamo)
        {
            if (id != prestamo.Id)
            {
                return BadRequest();
            }

            prestamo.Aprobado = true;
            prestamo.Denegado = false;

            

            var documentoItem = await _context.DocumentoItems.FindAsync(prestamo.DocumentoItemId);

            if (documentoItem.EsFisico == true)
            {
                documentoItem.Activo = false;
            }

            documentoItem.NumeroPrestamos++;

            _context.Entry(documentoItem).State = EntityState.Modified;

            _context.Entry(prestamo).State = EntityState.Modified;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrestamoExists(id))
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

        [Authorize]
        [HttpPost("Denegar")]
        public async Task<ActionResult<Prestamo>> DenegarPrestamo([FromForm] Prestamo prestamo)
        {
            prestamo.Aprobado = false;
            prestamo.Denegado = true;


            _context.Prestamos.Add(prestamo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPrestamo", new { id = prestamo.Id }, prestamo);
        }

        [Authorize]
        [HttpPost("Devolver")]
        public async Task<ActionResult<Prestamo>> DevolverPrestamo([FromForm] Prestamo prestamo)
        {
            prestamo.Devuelto = true;
            prestamo.


            _context.Prestamos.Add(prestamo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPrestamo", new { id = prestamo.Id }, prestamo);
        }

        // DELETE: api/Prestamo/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrestamo(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo == null)
            {
                return NotFound();
            }

            _context.Prestamos.Remove(prestamo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PrestamoExists(int id)
        {
            return _context.Prestamos.Any(e => e.Id == id);
        }
    }
}
