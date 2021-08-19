using System;
using System.Collections.Generic;

#nullable disable

namespace BibliotecaUDEO.Models
{
    public partial class DocumentoItem
    {
        public DocumentoItem()
        {
            Prestamos = new HashSet<Prestamo>();
        }

        public int Id { get; set; }
        public bool EsFisico { get; set; }
        public string LibroUrl { get; set; }
        public int NumeroPrestamos { get; set; }
        public bool Activo { get; set; }
        public int DocumentoId { get; set; }
        public int SedeId { get; set; }

        public virtual Documento Documento { get; set; }
        public virtual Sede Sede { get; set; }
        public virtual ICollection<Prestamo> Prestamos { get; set; }
    }
}
