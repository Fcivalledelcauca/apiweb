using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Entidades
{
    public class MunicipioTemaDisplay
    {
        public int IdMunicipioTema { get; set; }
        public int? IdMunicipio { get; set; }
        public int? IdTema { get; set; }
        public bool? activo { get; set; }
    }
}
