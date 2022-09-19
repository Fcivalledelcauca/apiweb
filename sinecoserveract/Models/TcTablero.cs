using System;
using System.Collections.Generic;

#nullable disable

namespace sinecoserveract.Models
{
    public partial class TcTablero
    {
        public int IdTablero { get; set; }
        public string Lote { get; set; }
        public string Nombre { get; set; }
        public bool? Activo { get; set; }
    }
}
