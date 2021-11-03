using System;
using System.Collections.Generic;

#nullable disable

namespace BibliotecaUDEO.Models
{
    public partial class Prestamo
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public bool? Perdida { get; set; }
        public bool? Danio { get; set; }
        public bool? Devuelto { get; set; }
        public bool? Aprobado { get; set; }
        public bool? Denegado { get; set; }
        public int DiasAtraso { get; set; }
        public int DocumentoItemId { get; set; }
        public int UsuarioId { get; set; }

        public virtual DocumentoItem DocumentoItem { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
