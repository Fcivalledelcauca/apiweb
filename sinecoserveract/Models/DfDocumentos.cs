using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Models
{
    public class DfDocumentos
    {
        public int IdDocumento { get; set; }
        public int IdGrupo { get; set; }
        public int? IdCia { get; set; }
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public string? CodMunicipio { get; set; }
        public string? NombreArchivo { get; set; }
        public string? Archivo { get; set; }
        public bool? Activo { get; set; }
    }
}
