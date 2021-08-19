using System;
using System.Collections.Generic;

#nullable disable

namespace BibliotecaUDEO.Models
{
    public partial class TipoDocumento
    {
        public TipoDocumento()
        {
            Documentos = new HashSet<Documento>();
        }

        public int Id { get; set; }
        public string Tipo { get; set; }

        public virtual ICollection<Documento> Documentos { get; set; }
    }
}
