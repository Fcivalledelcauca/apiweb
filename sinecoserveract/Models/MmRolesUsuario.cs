using System;
using System.Collections.Generic;

#nullable disable

namespace sinecoserveract.Models
{
    public partial class MmRolesUsuario
    {
        public int IdRolUsuario { get; set; }
        public string TipoRol { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdCia { get; set; }
        public string IdCo { get; set; }
        public string IdTpv { get; set; }
    }
}
