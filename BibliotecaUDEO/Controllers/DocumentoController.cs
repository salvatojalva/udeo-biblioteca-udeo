using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaUDEO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace BibliotecaUDEO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentoController : ControllerBase
    {
        private readonly BibliotecaUDEOContext _context;
        public static IWebHostEnvironment _environment;

        public DocumentoController(BibliotecaUDEOContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        [HttpPost("StoreDocumento")]
        public async Task<ActionResult<Usuario>> PostDocumentormData([FromForm] DocumentoFormData documentoFormData)
       {
            int anio_id=0;
            int categoria_id=0,division_id=0, tipo_id=0,carrear_id=0,sede_id=0;
          if(documentoFormData.anio_id==null)
            {
                Anio anio = new Anio();
                anio.Nombre = documentoFormData.nombreanio;
                _context.Anios.Add(anio);
                await _context.SaveChangesAsync();
                anio_id = anio.Id;
            }
            else
            {
                anio_id = (int)documentoFormData.anio_id;
            }

          if(documentoFormData.categoria_id==null)
            {
                Categorium categoria = new Categorium();
                categoria.Nombre = documentoFormData.categorianombre;
                _context.Categoria.Add(categoria);
                await _context.SaveChangesAsync();
                categoria_id = categoria.Id;
            }
            else
            {
                categoria_id = (int)documentoFormData.categoria_id;
            }


            if (documentoFormData.division_id == null)
            {
                Division division = new Division();
                division.Nombre = documentoFormData.divisionnombre;
                _context.Divisions.Add(division);
                await _context.SaveChangesAsync();
                division_id = division.Id;
            }
            else
            {
                division_id = (int)documentoFormData.division_id;
            }
            if (documentoFormData.tipo_id == null)
            {
                TipoDocumento tipo = new TipoDocumento();
                tipo.Tipo = documentoFormData.tiponombre;
                _context.TipoDocumentos.Add(tipo);
                await _context.SaveChangesAsync();
               tipo_id = tipo.Id;
            }
            else
            {
                tipo_id = (int)documentoFormData.tipo_id;
            }

            if (documentoFormData.carrera_id == null)
            {
                Carrera carrera= new Carrera();
                carrera.Nombre = documentoFormData.carreanombre;
                _context.Carreras.Add(carrera);
                await _context.SaveChangesAsync();
                carrear_id = carrera.Id;
            }
            else
            {
                carrear_id = (int)documentoFormData.carrera_id;
            }
           if (documentoFormData.sede_id == null)
            {
                Sede sede = new Sede();
                sede.Nombre = documentoFormData.sedenombre;
                _context.Sedes.Add(sede);
                await _context.SaveChangesAsync();
                carrear_id = sede.Id;
            }
            else
            {
                sede_id = (int)documentoFormData.sede_id;
            }



           string endpointimagen;
            endpointimagen = "";

            if (documentoFormData.imagen.Length > 0)
            {
                if (!Directory.Exists(_environment.WebRootPath + "\\Uplods\\"))
                {
                    Directory.CreateDirectory(_environment.WebRootPath + "\\Uplods\\");
                }
                DateTime foo = DateTime.Now;
                long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();
                string[] formatosadmitidos = { ".PNG", ".JPG" };
                string FormatoImagen = Path.GetExtension(documentoFormData.imagen.FileName).ToUpper();

                if (formatosadmitidos.Contains(FormatoImagen))
                {
                    string NombreImagen = documentoFormData.imagen.FileName;
                    NombreImagen = Convert.ToString(unixTime) + FormatoImagen;
                    var filpath = _environment.WebRootPath + "\\Uplods\\" + NombreImagen;

                    using (FileStream fileStream = System.IO.File.Create(filpath))
                    {
                       documentoFormData.imagen.CopyTo(fileStream);
                        fileStream.Flush();
                        endpointimagen = NombreImagen;
                    }
                }
            }

            Documento documento = new Documento();

            documento.Codigo = documentoFormData.codigo;
            documento.Titulo = documentoFormData.titulo;
            documento.Creado = documentoFormData.FechaCreado;
            documento.Modificado = documentoFormData.FechaModificado;
            documento.Image = endpointimagen;
            documento.AnioId = anio_id;
            documento.CategoriaId = categoria_id;
            documento.DivisionId = division_id;
            documento.TipoDocumentoId = tipo_id;
            documento.CarreraId = carrear_id;

            _context.Documentos.Add(documento);
            await _context.SaveChangesAsync();



            DocumentoItem documentoitem = new DocumentoItem();
            string endpointArchivo;
            endpointArchivo = "";

            if (documentoFormData.archivo!=null)
            {
                if (!Directory.Exists(_environment.WebRootPath + "\\Uplods\\"))
                {
                    Directory.CreateDirectory(_environment.WebRootPath + "\\Uplods\\");
                }
                DateTime foo = DateTime.Now;
                long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();
                string[] formatosadmitidos = { ".PDF"};
                string FormatoArchivo = Path.GetExtension(documentoFormData.archivo.FileName).ToUpper();

                if (formatosadmitidos.Contains(FormatoArchivo))
                {
                    string NombreArchivo = documentoFormData.archivo.FileName;
                    NombreArchivo = Convert.ToString(unixTime) + FormatoArchivo;
                    var filpath = _environment.WebRootPath + "\\Uplods\\" + NombreArchivo;

                    using (FileStream fileStream = System.IO.File.Create(filpath))
                    {
                        documentoFormData.archivo.CopyTo(fileStream);
                        fileStream.Flush();
                        endpointArchivo = NombreArchivo;
                    }
                }

            //    DocumentoItem documentoitem = new DocumentoItem();
                documentoitem.EsFisico = false;
                documentoitem.LibroUrl = endpointArchivo;
                documentoitem.NumeroPrestamos = 0;
                documentoitem.Activo = true;
                documentoitem.DocumentoId = documento.Id;
                documentoitem.SedeId = sede_id;
              

            }
            else
            {
                documentoitem.EsFisico = true;
                documentoitem.LibroUrl =endpointArchivo;
                documentoitem.NumeroPrestamos = 0;
                documentoitem.Activo = true;
                documentoitem.DocumentoId = documento.Id;
                documentoitem.SedeId = sede_id;
            }

            _context.DocumentoItems.Add(documentoitem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDocumento", new { id = documento.Id }, documento);
        }




        // GET: api/Documento
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] string filterByTitle, int? page, int? records)
        {
            int _page = page ?? 1;
            int _records = records ?? 7;
            int total_page;
            int totalCount;
            List<Documento> documento = new List<Documento>();
            if (filterByTitle != null)
            {
                decimal total_records = await _context.Documentos.Where(x => x.Titulo.Contains(filterByTitle)).CountAsync();
                totalCount = Convert.ToInt32(total_records);
                total_page = Convert.ToInt32(Math.Ceiling(total_records / _records));
                documento = await _context.Documentos.Where(x => x.Titulo.Contains(filterByTitle)).Skip((_page - 1) * _records).Take(_records).ToListAsync();
            }
            else
            {
                decimal total_records = await _context.Documentos.CountAsync();
                totalCount = Convert.ToInt32(total_records);
                total_page = Convert.ToInt32(Math.Ceiling(total_records / _records));
                documento = await _context.Documentos.Skip((_page - 1) * _records).Take(_records).ToListAsync();
            }
            return Ok(new
            {
                totalCount = totalCount,
                records = documento,
                current_page = _page
            });


        }

        // GET: api/Documento/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Documento>> GetDocumento(int id)
        {
            var documento = await _context.Documentos.FindAsync(id);

            if (documento == null)
            {
                return NotFound();
            }

            return documento;
        }

        // PUT: api/Documento/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocumento(int id, Documento documento)
        {
            if (id != documento.Id)
            {
                return BadRequest();
            }

            _context.Entry(documento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentoExists(id))
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

        // POST: api/Documento
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Documento>> PostDocumento(Documento documento)
        {
            _context.Documentos.Add(documento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDocumento", new { id = documento.Id }, documento);
        }

        // DELETE: api/Documento/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocumento(int id)
        {
            var documento = await _context.Documentos.FindAsync(id);
            if (documento == null)
            {
                return NotFound();
            }

            _context.Documentos.Remove(documento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocumentoExists(int id)
        {
            return _context.Documentos.Any(e => e.Id == id);
        }

        public class DocumentoFormData
        {
            public string codigo { get; set; }
            public string titulo { get; set; }
            public DateTime FechaCreado { get; set; }
            public DateTime FechaModificado { get; set; }
            public IFormFile imagen { get; set; }                     
            public int? anio_id { get; set; }
            public string? nombreanio { get; set; }
            public int? categoria_id { get; set; }
            public string? categorianombre { get; set; }
            public int? division_id { get; set; }
            public string? divisionnombre { get; set; }
            public int? tipo_id { get; set; }
            public string? tiponombre { get; set; }
            public int? carrera_id { get; set; }
            public string? carreanombre { get; set; }



            public int? sede_id { get; set; }
            public string? sedenombre { get; set; }
            public IFormFile archivo { get; set; }

        }

        public class DocumentoItemFD
        {
           
        }
    }
}
