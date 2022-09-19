using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Entidades
{
    public class EventoArchivoDisplay
    {
        public int IdEvento { get; set; }
        public int IdTema { get; set; }
        public int IdTemario { get; set; }
        public string? Archivo { get; set; }
        public string? NombreArchivo { get; set; }
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public IFormFile? File { get; set; }
    }
}
