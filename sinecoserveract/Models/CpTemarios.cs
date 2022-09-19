using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Models
{
    public class CpTemarios
    {
        public int IdTemario { get; set; }
        public int? IdTema { get; set; }
        public string? Temario { get; set; }
        public bool? Activo { get; set; }
    }
}
