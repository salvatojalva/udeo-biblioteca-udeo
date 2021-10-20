using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BibliotecaUDEO.Models;
using Microsoft.AspNetCore.Http;

namespace BibliotecaUDEO.Requests
{
    public class DocumentoRequest
    {
        public IFormFile Portada { get; set; }
        public IFormFile Documento { get; set; }

        public string Codigo { get; set; }
        public string Titulo { get; set; }
        public int CantidadEjemplares { get; set; }


        public virtual IdNombreRequest Anio { get; set; }
        public virtual IdNombreRequest Carrera { get; set; }
        public virtual IdNombreRequest Categoria { get; set; }
        public virtual IdNombreRequest Division { get; set; }
        public virtual IdNombreRequest TipoDocumento { get; set; }
        public virtual SedeRequest Sede { get; set; }
        public virtual IdNombreRequest Editorial { get; set; }

        public virtual ICollection<IdNombreRequest> Autores { get; set; }
        public virtual ICollection<IdNombreRequest> Tags { get; set; }
    }

    public class IdNombreRequest { 
        public int? Id { get; set; }
        public string Nombre { get; set; }
    }

    public class SedeRequest
    {
        public int? Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
    }
}
