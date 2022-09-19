using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Models
{
    public class MmPermisosRole
    {
        public int IdPermisoRole { get; set; }
        public int? IdCia { get; set; }
        public string IdCo { get; set; }
        public string IdTpv { get; set; }
        public string TipoRol { get; set; }
        public int? IdModulo { get; set; }
        public int? IdOpciones { get; set; }
        public bool? Activo { get; set; }
    }
}
