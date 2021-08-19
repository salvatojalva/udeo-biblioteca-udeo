using System;
using System.Collections.Generic;

#nullable disable

namespace BibliotecaUDEO.Models
{
    public partial class AutorDocumento
    {
        public int Id { get; set; }
        public int DocumentoId { get; set; }
        public int AutorId { get; set; }

        public virtual Autor Autor { get; set; }
        public virtual Documento Documento { get; set; }
    }
}
