using System;
using System.Collections.Generic;

#nullable disable

namespace sinecoserveract.Models
{
    public partial class TcPromedioDimensione
    {
        public int IdPromedioDim { get; set; }
        public int IdTablero { get; set; }
        public string Lote { get; set; }
        public string Tipo { get; set; }
        public decimal? D1TalentoHumano { get; set; }
        public decimal? D2DireccionamientoEstrategicoPlaneacion { get; set; }
        public decimal? D3GestionResultadosValores { get; set; }
        public decimal? D4EvaluacionResultados { get; set; }
        public decimal? D5InformacionComunicacion { get; set; }
        public decimal? D6GestionConocimiento { get; set; }
        public decimal? D7Control { get; set; }
        public decimal? Idi { get; set; }
        public bool? Activo { get; set; }
    }
}
