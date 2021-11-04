using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaUDEO.Requests
{
    public class PrestamoRequest
    {

        public int? Id { get; set; }
        public int? DocumentoId { get; set; }
        public int? UsuarioId { get; set; }
        public bool EsDigital { get; set; }
    }
}
