using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Entidades
{
    public class User
    {
        public string NombreUsuario { get; set; }

        public string Email { get; set; }
        
        public string Password { get; set; }

        public string NombreCompleto { get; set; }

        public string DocumentoIdentidad { get; set; }

        public string Telefono { get; set; }

        public string Direccion { get; set; }

        public int IdMunicipio { get; set; }

        public string Municipio { get; set; }

        public string idRol { get; set; }

        public string TipoRol { get; set; }

        public bool activo { get; set; }

    }
}
