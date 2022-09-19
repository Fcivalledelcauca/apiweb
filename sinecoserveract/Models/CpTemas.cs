using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Models
{
    public class CpTemas
    {
        public int IdTema { get; set; }
        public int? IdMunicipio { get; set; }
        public string? Tema { get; set; }
        public bool? Activo { get; set; }
    }
}
