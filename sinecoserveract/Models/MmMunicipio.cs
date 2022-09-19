using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Models
{
    public class MmMunicipio
    {
        public int IdMunicipio { get; set; }
        public string Nombre { get; set; }
        public string CodigoDane { get; set; }
        public string Region { get; set; }
        public string Categoria_Ruralidad { get; set; }
        public string Dotaciones_Iniciales { get; set; }
        public string Grupo_Par { get;set;}
        public bool? Activo { get; set; }
    }
}
