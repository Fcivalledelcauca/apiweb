using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Models
{
    public class CpExamenPresentado
    {
        public int IdExamenPresentado { get; set; }
        public string Usuario { get; set; }
        public int IdExamen { get; set; }
        public decimal PuntosMin { get; set; }
        public decimal Puntuacion { get; set; }
        public decimal Can_Ok { get; set; }
        public decimal Can_Fail { get; set; }
        public bool? Aprobo { get; set; }
    }
}
