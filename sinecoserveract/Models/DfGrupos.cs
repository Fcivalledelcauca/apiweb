using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Models
{
    public class DfGrupos
    {
        public int IdGrupo { get; set; }
        public int? Orden { get; set; }
        public int? IdCia { get; set; }
        public string? Nombre_Grupo { get; set; }
        public string? Descripcion { get; set; }
        public string? Icono { get; set; }
        public string? ClaseCSS { get; set; }
        public bool? Activo { get; set; }
    }

}
