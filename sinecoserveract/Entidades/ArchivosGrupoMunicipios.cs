using Microsoft.AspNetCore.Http;
using sinecoserveract.Validaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Entidades
{
    public class ArchivosGrupoMunicipios
    {
        public string Municipio { get; set; }

        public string Grupo { get; set; }

        public int IdGrupo { get; set; }

        public string titulo { get; set; }

        [PesoImagenValid(PesoMaximo: 10)]
        [TipoArchivoValid(grupoTipoArchivo: GrupoTipoArchivo.Imagen)]
        public IFormFile File { get; set; }
    }
}
