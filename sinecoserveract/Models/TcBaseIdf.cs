using System;
using System.Collections.Generic;

#nullable disable

namespace sinecoserveract.Models
{
    public partial class TcBaseIdf
    {
        public int IdBaseIdf { get; set; }
        public int? IdTablero { get; set; }
        public string Lote { get; set; }
        public string CodigoDane { get; set; }
        public string Municipio { get; set; }
        public decimal? CaDependenciaTransferencia { get; set; }
        public decimal? CaRelevanciaFbkFijo { get; set; }
        public decimal? CaEndeudamientoLargoPlazo { get; set; }
        public decimal? CaAhorroCorriente { get; set; }
        public decimal? CaBalancePrimario { get; set; }
        public decimal? Resultado { get; set; }
        public decimal? CaResultado { get; set; }
        public decimal? CaHolgura { get; set; }
        public decimal? CaCapacidadEjecucionIngresos { get; set; }
        public decimal? caCapacidadEjecucioInversion { get; set; }
        public decimal? BonificacionEsfuerzoPropio { get; set; }
        public decimal? Gestion { get; set; }
        public decimal? GestionBonos { get; set; }
        public decimal? CaGestion { get; set; }
        public decimal? NuevoIdf { get; set; }
        public decimal? NuevoIdfSb { get; set; }
        public string Rango { get; set; }
        public bool? Activo { get; set; }
    }
}
