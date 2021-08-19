using System;
using System.Collections.Generic;

#nullable disable

namespace BibliotecaUDEO.Models
{
    public partial class Autor
    {
        public Autor()
        {
            AutorDocumentos = new HashSet<AutorDocumento>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }

        public virtual ICollection<AutorDocumento> AutorDocumentos { get; set; }
    }
}
