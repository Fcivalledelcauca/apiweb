using Microsoft.AspNetCore.Http;
using sinecoserveract.Validaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Entidades
{
    public class formArchivos
    {
        public string Usuario { get; set; }

        [PesoImagenValid(PesoMaximo: 10)]
        [TipoArchivoValid(grupoTipoArchivo: GrupoTipoArchivo.Imagen)]
        public IFormFile Imagen { get; set; }
    }
}
