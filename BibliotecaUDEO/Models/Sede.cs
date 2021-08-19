using System;
using System.Collections.Generic;

#nullable disable

namespace BibliotecaUDEO.Models
{
    public partial class Sede
    {
        public Sede()
        {
            DocumentoItems = new HashSet<DocumentoItem>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }

        public virtual ICollection<DocumentoItem> DocumentoItems { get; set; }
    }
}
