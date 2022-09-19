using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Entidades
{
    public class RespuestaAutenticacion
    {
        public bool Ok { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }
        public string Token { get; set; }
        public List<Menu> Menu { get; set; }
        public DateTime Expiracion { get; set; }
    }
}