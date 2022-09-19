using System;
using System.Collections.Generic;

#nullable disable

namespace sinecoserveract.Models
{
    public partial class MmUsuariosSistema
    {
        public int IdUsuario { get; set; }
        public int? IdTercero { get; set; }
        public int? IdCia { get; set; }
        public string NombreUsuario { get; set; }
        public string ClaveUsuario { get; set; }
        public string Imagen { get; set; }
        public bool? Activo { get; set; }
    }
}
