using System;
using System.Collections.Generic;

#nullable disable

namespace BibliotecaUDEO.Models
{
    public partial class TagDocumento
    {
        public int Id { get; set; }
        public int DocumentoId { get; set; }
        public int TagId { get; set; }

        public virtual Documento Documento { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
