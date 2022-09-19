using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Entidades
{
    public class EventoMaterial
    {
        public int? IdCia { get; set; }
        public int? IdMunicipio { get; set; }
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

        public List<Temas> Temas { get; set; }

    }

    public class Temas { 
        public int IdTema { get; set; }
        public string NombreTema { get; set; }
        public List<Temarios> Temarios { get; set; }
    }

    public class Temarios { 
        public int IdTemario { get; set; }
        public string NombreTemario { get; set; }
        public List<ArchivosGuardados> Archivos { get; set; }
    }

    public class ArchivosGuardados { 
        public string Archivo { get; set; }
        public string NombreArchivo { get; set; }
    }
}
