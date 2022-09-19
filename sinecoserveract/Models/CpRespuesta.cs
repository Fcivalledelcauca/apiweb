using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Models
{
    public class CpRespuesta
    {
        public int IdRespuesta { get; set; }
        public int IdPregunta { get; set; }
        public string Respuesta { get; set; }
        public int Correcta { get; set; }
    }
}
