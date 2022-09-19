using System;
using System.Collections.Generic;

#nullable disable

namespace sinecoserveract.Models
{
    public partial class TcResumenMdm
    {
        public int IdResumen { get; set; }
        public int IdTablero { get; set; }
        public string Lote { get; set; }
        public string Grupo { get; set; }
        public decimal? Mdm { get; set; }
        public decimal? Gestion { get; set; }
        public decimal? Resultado { get; set; }
        public bool? Activo { get; set; }
    }
}
