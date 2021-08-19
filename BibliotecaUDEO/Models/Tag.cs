using System;
using System.Collections.Generic;

#nullable disable

namespace BibliotecaUDEO.Models
{
    public partial class Tag
    {
        public Tag()
        {
            TagDocumentos = new HashSet<TagDocumento>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<TagDocumento> TagDocumentos { get; set; }
    }
}
