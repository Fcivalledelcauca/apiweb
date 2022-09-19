using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Entidades
{
    public class ExamenDisplay
    {
        public int IdTema { get; set; }
        public string Nombre { get; set; }
        public decimal Puntuacion { get; set; }
        public string Instrucciones { get; set; }
        public bool Activo { get; set; }
    }
}
