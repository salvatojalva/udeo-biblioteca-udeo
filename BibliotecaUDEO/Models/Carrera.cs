using System;
using System.Collections.Generic;

#nullable disable

namespace BibliotecaUDEO.Models
{
    public partial class Carrera
    {
        public Carrera()
        {
            Documentos = new HashSet<Documento>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Documento> Documentos { get; set; }
    }
}
