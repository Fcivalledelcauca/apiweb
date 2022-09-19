using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Models
{
    public class MmOpcionesModulo
    {
        public int IdOpciones { get; set; }
        public int? IdModulo { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Icon { get; set; }
        public string Link { get; set; }
        public string NombreOpcion { get; set; }
        public string NombreControl { get; set; }
        public bool? Activo { get; set; }
    }
}
