using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Models
{
    public class CpRespuestaPresentada
    {
        public int IdRespuestaPresentada { get; set; }
        public int IdExamenPresentado { get; set; }
        public int IdRespuesta { get; set; }
        public decimal Puntos { get; set; }
    }
}

