using System;
using System.Collections.Generic;

#nullable disable

namespace BibliotecaUDEO.Models
{
    public partial class Documento
    {
        public Documento()
        {
            AutorDocumentos = new HashSet<AutorDocumento>();
            DocumentoItems = new HashSet<DocumentoItem>();
            TagDocumentos = new HashSet<TagDocumento>();
        }

        public int Id { get; set; }
        public int? EditorialId { get; set; }
        public string Codigo { get; set; }
        public string Titulo { get; set; }
        public DateTime Creado { get; set; }
        public DateTime Modificado { get; set; }
        public string Image { get; set; }
        public int AnioId { get; set; }
        public int CategoriaId { get; set; }
        public int DivisionId { get; set; }
        public int TipoDocumentoId { get; set; }
        public int CarreraId { get; set; }

        public virtual Anio Anio { get; set; }
        public virtual Carrera Carrera { get; set; }
        public virtual Categorium Categoria { get; set; }
        public virtual Division Division { get; set; }
        public virtual Editorial Editorial { get; set; }
        public virtual TipoDocumento TipoDocumento { get; set; }
        public virtual ICollection<AutorDocumento> AutorDocumentos { get; set; }
        public virtual ICollection<DocumentoItem> DocumentoItems { get; set; }
        public virtual ICollection<TagDocumento> TagDocumentos { get; set; }
    }
}
