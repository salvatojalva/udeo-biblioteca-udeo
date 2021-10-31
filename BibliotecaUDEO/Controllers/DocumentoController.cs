using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaUDEO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using BibliotecaUDEO.Requests;
using Microsoft.AspNetCore.Http;

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

        [HttpPost("UploadDocumento")]
        public async Task<ActionResult<Documento>> UploadDocumento([FromForm] DocumentoRequest request)
        {
            int? carrera_id, 
                anio_id, 
                categoria_id, 
                division_id,
                sede_id,
                documento_id,
                editorial_id = 0,
                tipo_documento_id;
            string documento_nombre;
            bool editorial_id_null = false;


            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {

                if (request.Carrera.Id == null)
                {
                    Carrera carrera = new Carrera();
                    carrera.Nombre = request.Carrera.Nombre;
                    _context.Carreras.Add(carrera);
                    await _context.SaveChangesAsync();
                    carrera_id = carrera.Id;
                }
                else carrera_id = request.Carrera.Id;

                if (request.Anio.Id == null)
                {
                    Anio entidad = new Anio();
                    entidad.Nombre = request.Anio.Nombre;
                    _context.Anios.Add(entidad);
                    await _context.SaveChangesAsync();
                    anio_id = entidad.Id;
                }
                else anio_id = request.Anio.Id;

                if (request.Categoria.Id == null)
                {
                    Categorium entidad = new Categorium();
                    entidad.Nombre = request.Categoria.Nombre;
                    _context.Categoria.Add(entidad);
                    await _context.SaveChangesAsync();
                    categoria_id = entidad.Id;
                }
                else categoria_id = request.Categoria.Id;

                if (request.Division.Id == null)
                {
                    Division entidad = new Division();
                    entidad.Nombre = request.Division.Nombre;
                    _context.Divisions.Add(entidad);
                    await _context.SaveChangesAsync();
                    division_id = entidad.Id;
                }
                else division_id = request.Division.Id;

                if (request.TipoDocumento.Id == null)
                {
                    TipoDocumento entidad = new TipoDocumento();
                    entidad.Tipo = request.TipoDocumento.Nombre;
                    _context.TipoDocumentos.Add(entidad);
                    await _context.SaveChangesAsync();
                    tipo_documento_id = entidad.Id;
                }
                else tipo_documento_id = request.TipoDocumento.Id;

                if (request.Editorial.Nombre != null)
                {
                    Editorial entidad = new Editorial();
                    entidad.Nombre = request.Editorial.Nombre;
                    _context.Editorials.Add(entidad);
                    await _context.SaveChangesAsync();
                    editorial_id = entidad.Id;
                } else editorial_id_null = true;
                
                

                Documento documento = new Documento();
                documento.Codigo = request.Codigo;
                documento.Titulo = request.Titulo;
                documento.Image = getPortadaFileName(request.Portada);
                documento.Creado = DateTime.Now;
                documento.Modificado = DateTime.Now;
                documento.AnioId = (int)anio_id;
                documento.CarreraId = (int)carrera_id;
                documento.CategoriaId = (int)categoria_id;
                documento.TipoDocumentoId = (int)tipo_documento_id;
                documento.DivisionId = (int)division_id;
                documento.EditorialId = editorial_id_null ? null : (int)editorial_id;

                _context.Documentos.Add(documento);
                await _context.SaveChangesAsync();
                documento_id = documento.Id;

                if (request.Sede.Id == null)
                {
                    Sede entidad = new Sede();
                    entidad.Nombre = request.Sede.Nombre;
                    entidad.Direccion = request.Sede.Direccion;
                    _context.Sedes.Add(entidad);
                    await _context.SaveChangesAsync();
                    sede_id = entidad.Id;
                }
                else sede_id = request.Sede.Id;

                documento_nombre = getDocumentoFileName(request.Documento);


                if(documento_nombre != null)
                {
                    await createEjemplar(false,documento_nombre, documento_id, sede_id);
                }

                for (int iteracion = 0; iteracion < request.CantidadEjemplares; iteracion++)
                {
                    await createEjemplar(true, "", documento_id, sede_id);
                }

                foreach (var autor in request.Autores)
                {
                    int? autor_id;

                    if (autor.Id == null)
                    {
                        Autor entidad = new Autor();
                        entidad.Nombre = autor.Nombre;
                        _context.Autors.Add(entidad);
                        await _context.SaveChangesAsync();
                        autor_id = entidad.Id;
                    }
                    else autor_id = autor.Id;

                    await asignarCrearAutores((int)autor_id, (int)documento_id);
                }

                foreach (var tag in request.Tags)
                {
                    int? tag_id;

                    if (tag.Id == null)
                    {
                        Tag entidad = new Tag();
                        entidad.Nombre = tag.Nombre;
                        _context.Tags.Add(entidad);
                        await _context.SaveChangesAsync();
                        tag_id = entidad.Id;
                    }
                    else tag_id = tag.Id;

                    await asignarCrearTags((int)tag_id, (int)documento_id);
                }


                dbContextTransaction.Commit();
                return documento;
            }

            
        }


        private async Task<ActionResult<AutorDocumento>> asignarCrearAutores(int autor_id, int documento_id)
        {
            AutorDocumento autor = new AutorDocumento();

            autor.AutorId = autor_id;
            autor.DocumentoId = documento_id;

            _context.AutorDocumentos.Add(autor);
            await _context.SaveChangesAsync();

            return autor;

        }

        private async Task<ActionResult<TagDocumento>> asignarCrearTags(int tag_id, int documento_id)
        {
            TagDocumento tag = new TagDocumento();

            tag.TagId = tag_id;
            tag.DocumentoId = documento_id;

            _context.TagDocumentos.Add(tag);
            await _context.SaveChangesAsync();

            return tag;

        }


        private async Task<ActionResult<DocumentoItem>> createEjemplar(bool fisico, string nombre, int? documento_id, int? sede_id)
        {
            DocumentoItem ejemplar = new DocumentoItem();

            ejemplar.EsFisico = fisico;
            ejemplar.LibroUrl = nombre;
            ejemplar.NumeroPrestamos = 0;
            ejemplar.Activo = true;
            ejemplar.DocumentoId = (int)documento_id;
            ejemplar.SedeId = (int)sede_id;

            _context.DocumentoItems.Add(ejemplar);
            await _context.SaveChangesAsync();

            return ejemplar;

        }

        private string getDocumentoFileName(IFormFile documento)
        {
            string documento_nombre = "";

            if (!Directory.Exists(_environment.WebRootPath + "\\Files\\"))
            {
                Directory.CreateDirectory(_environment.WebRootPath + "\\Files\\");
            }
            DateTime foo = DateTime.Now;
            long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();
            string[] formatosadmitidos = { ".PDF" };
            string FormatoArchivo = Path.GetExtension(documento.FileName).ToUpper();

            if (formatosadmitidos.Contains(FormatoArchivo))
            {
                string NombreArchivo = documento.FileName;
                NombreArchivo = Convert.ToString(unixTime) + FormatoArchivo;
                var filpath = _environment.WebRootPath + "\\Files\\" + NombreArchivo;

                using (FileStream fileStream = System.IO.File.Create(filpath))
                {
                    documento.CopyTo(fileStream);
                    fileStream.Flush();
                    documento_nombre = NombreArchivo;
                }
            }

            return documento_nombre;
        }

        private string getPortadaFileName(IFormFile documento)
        {
            string documento_nombre = "";

            if (!Directory.Exists(_environment.WebRootPath + "\\Images\\"))
            {
                Directory.CreateDirectory(_environment.WebRootPath + "\\Images\\");
            }
            DateTime foo = DateTime.Now;
            long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();
            string[] formatosadmitidos = { ".PNG", ".JPG" };
            string FormatoImagen = Path.GetExtension(documento.FileName).ToUpper();

            if (formatosadmitidos.Contains(FormatoImagen))
            {
                string NombreImagen = documento.FileName;
                NombreImagen = Convert.ToString(unixTime) + FormatoImagen;
                var filpath = _environment.WebRootPath + "\\Images\\" + NombreImagen;

                using (FileStream fileStream = System.IO.File.Create(filpath))
                {
                    documento.CopyTo(fileStream);
                    fileStream.Flush();
                    documento_nombre = NombreImagen;
                }
            }

            return documento_nombre;
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
                documento = await _context
                    .Documentos
                    .Where(x => x.Titulo.Contains(filterByTitle))
                    .OrderBy(x =>x.Titulo)
                    .Skip((_page - 1) * _records).Take(_records)
                    .ToListAsync();
            }
            else
            {
                decimal total_records = await _context.Documentos.CountAsync();
                totalCount = Convert.ToInt32(total_records);
                total_page = Convert.ToInt32(Math.Ceiling(total_records / _records));
                documento = await _context.Documentos.Skip((_page - 1) * _records).Take(_records).OrderBy(x => x.Titulo).ToListAsync();
            }


            return Ok(new
            {
                totalCount = totalCount,
                records = documento,
                current_page = _page
            });


        }

        // GET: api/Documento
        [HttpGet("Busqueda")]
        public async Task<ActionResult<Documento>> Get([FromQuery] string filtro1, string FilterByAutor, string FilterByTag, int? page, int? records)
        {
            int _page = page ?? 1;
            int _records = records ?? 7;
            int total_page;
            int totalCount;
            List<Documento> documento = new List<Documento>();
            List<Documento> docu = new List<Documento>();
            if (filtro1 != null)
            {
                docu = (from doc in _context.Documentos
                                  .Include(anio => anio.Anio)
                                  .Include(division => division.Division)
                                  .Include(categoria => categoria.Categoria)
                                  .Include(tipoDocumento => tipoDocumento.TipoDocumento)
                                  .Include(carrera => carrera.Carrera)
                                  .Include(editorial => editorial.Editorial)
                                   .Include(autordocumento=>autordocumento.AutorDocumentos)
                                   .ThenInclude(autor=>autor.Autor)
                        where doc.Anio.Nombre.Contains(filtro1)
                             || doc.Categoria.Nombre.Contains(filtro1)
                             || doc.Division.Nombre.Contains(filtro1)
                             || doc.TipoDocumento.Tipo.Contains(filtro1)
                             || doc.Carrera.Nombre.Contains(filtro1)
                             || doc.Editorial.Nombre.Contains(filtro1)
                        select doc).ToList();
            }
            else if (FilterByAutor != null)
            {
                docu = (from doc in _context.Documentos
                        join autdoc in _context.AutorDocumentos on doc.Id equals autdoc.DocumentoId
                        where autdoc.Autor.Nombre.Contains(FilterByAutor)
                        select   doc
                        
                        ).ToList();
            }
            else if (FilterByTag != null)
            {
                docu = (from doc in _context.Documentos
                        join tagdoc in _context.TagDocumentos on doc.Id equals tagdoc.DocumentoId
                        where tagdoc.Tag.Nombre.Contains(FilterByTag)
                        select doc).ToList();
            }
            if (docu.Count() != 0)
            {
                decimal total_records = docu.Count();
                totalCount = Convert.ToInt32(total_records);
                total_page = Convert.ToInt32(Math.Ceiling(total_records / _records));
                documento = docu.Skip((_page - 1) * _records).Take(_records).ToList();
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
                current_page = _page,
                totalCount = totalCount,
                result = documento
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
    }
}
