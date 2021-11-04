using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaUDEO.Models;
using Microsoft.AspNetCore.Authorization;
using BibliotecaUDEO.Requests;

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


        // GET: api/Prestamo/PorAprobar
        //[Authorize]
        [HttpGet("PorAprobar")]
        public async Task<ActionResult<Prestamo>> PorAprobar([FromQuery]  int? page, int? records)
        {
            int _page = page ?? 1;
            int _records = records ?? 1;
            int total_page;
            int totalCount;
            List<Prestamo> prestamo = new List<Prestamo>();

            
                decimal total_records = await _context.Prestamos
                    .Where(
                        x => x.Aprobado == false && 
                        x.Denegado == false &&
                        x.DocumentoItemId == null
                    )
                    .CountAsync();


                totalCount = Convert.ToInt32(total_records);
                total_page = Convert.ToInt32(Math.Ceiling(total_records / _records));
                prestamo = await _context.Prestamos
                    .Include(x => x.Documento)
                    .Include(x => x.Usuario)
                    .Where(
                        x => x.Aprobado == false &&
                        x.Denegado == false &&
                        x.DocumentoItemId == null
                    )
                    .Skip((_page - 1) * _records).Take(_records).ToListAsync();
            
            

            return Ok(new
            {
                current_page = _page,
                totalCount = totalCount,
                resul = prestamo
            });
        }

        // GET: api/Prestamo/FechaFin
        //[Authorize]
        [HttpGet("FechaFin")]
        public async Task<ActionResult<Prestamo>> Get([FromQuery] int UsuarioID, bool filterbydevuelto, int? page, int? records)
        {
            int _page = page ?? 1;
            int _records = records ?? 1;
            int total_page;
            int totalCount;
            List<Prestamo> prestamo = new List<Prestamo>();

                decimal total_records = await _context.Prestamos
                    .Where(
                        p => p.UsuarioId == UsuarioID && 
                        p.FechaFin <= DateTime.Now  &&
                        p.Aprobado == true && p.DocumentoItemId != null
                    )
                    .CountAsync();
                totalCount = Convert.ToInt32(total_records);
                total_page = Convert.ToInt32(Math.Ceiling(total_records / _records));
            prestamo = await _context.Prestamos
                 .Include(documento => documento.Documento)
                         .Where(
                                p => p.UsuarioId == UsuarioID &&
                                p.FechaFin <= DateTime.Now &&
                                p.Aprobado == true && p.DocumentoItemId != null
                            )
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
            var prestamo = await _context.Prestamos
                .Include(p => p.DocumentoItem)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

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
        public async Task<ActionResult<Prestamo>> PostPrestamo(PrestamoRequest prestamoRequest)
        {
            Prestamo prestamo = new Prestamo();

            prestamo.DocumentoId = (int)prestamoRequest.DocumentoId;
            prestamo.UsuarioId = (int)prestamoRequest.UsuarioId;
            prestamo.EsDigital = prestamoRequest.EsDigital;
            prestamo.Aprobado = true;
            prestamo.Denegado = false;

            prestamo.FechaFin = new DateTime().AddDays(7);
            prestamo.FechaInicio = new DateTime();

            var documentoItem = await _context.DocumentoItems.FirstOrDefaultAsync(x => x.Activo == true && x.EsFisico != prestamoRequest.EsDigital);

            if(documentoItem == null)
                return NotFound();


            if (documentoItem.EsFisico == true)
            {
                documentoItem.Activo = false;
            }
            prestamo.DocumentoItemId = documentoItem.Id;


            _context.Prestamos.Add(prestamo);
            _context.Entry(documentoItem).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPrestamo", new { id = prestamo.Id }, prestamo);
        }

        // POST: api/Prestamo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost("Solicitar")]
        public async Task<ActionResult<Prestamo>> SolicitarPrestamo(PrestamoRequest prestamoRequest)
        {

            Prestamo prestamo = new Prestamo();
            prestamo.Aprobado = false;
            prestamo.Denegado = false;
            prestamo.DocumentoId = (int)prestamoRequest.DocumentoId;
            prestamo.UsuarioId = (int)prestamoRequest.UsuarioId;
            prestamo.EsDigital = prestamoRequest.EsDigital;

            _context.Prestamos.Add(prestamo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPrestamo", new { id = prestamo.Id }, prestamo);
        }



        [Authorize]
        [HttpPost("Aprobar/{id}")]

        public async Task<IActionResult> AprobarPrestamo(int id, PrestamoRequest prestamoRequest)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);

            if (prestamo == null)
            {
                return NotFound();
            }


            prestamo.FechaFin = new DateTime().AddDays(7);
            prestamo.FechaInicio = new DateTime();

            prestamo.Aprobado = true;
            prestamo.Denegado = false;
            
            var documentoItem = await _context.DocumentoItems.FirstOrDefaultAsync(x => x.Activo == true && x.EsFisico != prestamo.EsDigital);

            if (documentoItem.EsFisico == true)
            {
                documentoItem.Activo = false;
            }

            prestamo.DocumentoItemId = documentoItem.Id;

            documentoItem.NumeroPrestamos++;


            _context.Entry(documentoItem).State = EntityState.Modified;

            _context.Entry(prestamo).State = EntityState.Modified;


            try
            {
                await _context.SaveChangesAsync();

                return Ok(new { result = prestamo });
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
        [HttpPost("Denegar/{id}")]
        public async Task<ActionResult<Prestamo>> DenegarPrestamo(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);

            if (prestamo == null)
            {
                return NotFound();
            }

            prestamo.Aprobado = false;
            prestamo.Denegado = true;


            _context.Entry(prestamo).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(new { result = prestamo });
        }

        [Authorize]
        [HttpPost("Devolver")]
        public async Task<ActionResult<Prestamo>> DevolverPrestamo(int id)
        {

            var prestamo = await _context.Prestamos.FindAsync(id);

            if (prestamo == null)
            {
                return NotFound();
            }

            prestamo.Devuelto = true;

            var documentoItem = await _context.DocumentoItems.FindAsync(prestamo.DocumentoItemId);

            if (documentoItem.EsFisico == true)
            {
                documentoItem.Activo = true;
                prestamo.FechaDevolucion = new DateTime();
            }

            _context.Entry(documentoItem).State = EntityState.Modified;

            _context.Entry(prestamo).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(new { result = prestamo });
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
