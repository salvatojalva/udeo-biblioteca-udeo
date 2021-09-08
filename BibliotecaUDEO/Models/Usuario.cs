using System;
using System.Collections.Generic;

#nullable disable

namespace BibliotecaUDEO.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Prestamos = new HashSet<Prestamo>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string GoogleId { get; set; }
        public bool Activo { get; set; }
        public string Rol { get; set; }
        public string Image { get; set; }

        public virtual ICollection<Prestamo> Prestamos { get; set; }
    }
}
