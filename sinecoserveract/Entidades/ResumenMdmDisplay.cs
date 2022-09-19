using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Entidades
{
    public class ResumenMdmDisplay
    {
        public int IdTablero { get; set; }
        public string Lote { get; set; }
        public string Grupo { get; set; }
        public decimal? Mdm { get; set; }
        public decimal? Gestion { get; set; }
        public decimal? Resultado { get; set; }
        public bool? Activo { get; set; }
    }
}
