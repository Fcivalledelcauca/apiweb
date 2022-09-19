using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Models
{
    public class CpEventos
    {
        public int IdEvento { get; set; }
        public int? IdCia { get; set; }
        public int IdMunicipio { get; set; }
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? Fecha_Ini { get; set; }
        public DateTime? Fecha_Fin { get; set; }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
        public int? TodoElDia { get; set; }
        public string? ColorEvento { get; set; }
        public string? Imagen { get; set; }
        public string? NombreArchivo { get; set; }
        public string? Direccion { get; set; }
        public string? Link { get; set; }
        public bool? Activo { get; set; }
    }
}
