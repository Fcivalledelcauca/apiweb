using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Models
{
    public class CpPregunta
    {
        public int IdPregunta { get; set; }
        public int IdExamen { get; set; }
        public string Pregunta { get; set; }
        public decimal Puntos { get; set; }
    }
}
