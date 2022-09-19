using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Entidades
{
    public class ClienteNuevo
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Identificacion { get; set; }
        [Required(ErrorMessage ="El campo {0} es obligatorio")]
        [MinLength(3,ErrorMessage ="El {0} debe tener como minimo tres caracteres")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string telefono { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [EmailAddress]
        public string Email { get; set; }

    }
}
