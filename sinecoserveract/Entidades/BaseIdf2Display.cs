using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Entidades
{
    public class BaseIdf2Display
    {
        public int? IdTablero { get; set; }
        public string Lote { get; set; }
        public string Tipo { get; set; }
        public string Grupo { get; set; }
        public decimal? DependenciaTransferencia { get; set; }
        public decimal? RelevanciaFbkFijo { get; set; }
        public decimal? EndeudamientoLargoPlazo { get; set; }
        public decimal? AhorroCorriente { get; set; }
        public decimal? BalancePrimario { get; set; }
        public decimal? Resultados { get; set; }
        public decimal? Idf { get; set; }
        public decimal? CaHolgura { get; set; }
        public decimal? CaCapacidadEjecucionIngresos { get; set; }
        public decimal? CaCapacidadEjecucionInversion { get; set; }
        public decimal? BonificacionEsfuerzoPropio { get; set; }
        public decimal? CaGestion { get; set; }
        public bool? Activo { get; set; }
    }
}
