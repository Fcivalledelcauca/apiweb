using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Models
{
    public class CpReporteExamenPresentado
    {
        public int Reg { get; set; }
        public int IdExamenPresentado { get; set; }
        public string Municipio { get; set; }
        public string Usuario { get; set; }
        public string Guia { get; set; }
        public string NomExamen { get; set; }
        public string Pregunta { get; set; }
        public string Respuesta { get; set; }
        public decimal Puntos { get; set; }
    }
}
