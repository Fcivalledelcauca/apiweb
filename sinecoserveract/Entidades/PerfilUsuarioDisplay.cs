﻿using Microsoft.AspNetCore.Http;
using sinecoserveract.Validaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Entidades
{
    public class PerfilUsuarioDisplay
    {
        public int IdCia { get; set; }
        public string IdCO { get; set; }

        public string NombreCompleto { get; set; }

        public string DocumentoIdentidad { get; set; }

        public string Telefono { get; set; }

        public string Direccion { get; set; }

        public string Email { get; set; }
        public int IdMunicipio { get; set; }

        public string Municipio { get; set; }

        public string Usuario { get; set; }

        [PesoImagenValid(PesoMaximo: 10)]
        [TipoArchivoValid(grupoTipoArchivo: GrupoTipoArchivo.Imagen)]
        public IFormFile Imagen { get; set; }



    }
}
