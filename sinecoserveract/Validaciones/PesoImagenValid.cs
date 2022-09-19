using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Validaciones
{
    public class PesoImagenValid:ValidationAttribute
    {
        private readonly int pesoMaximo;

        public PesoImagenValid(int PesoMaximo)
        {
            pesoMaximo = PesoMaximo;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) {
                return ValidationResult.Success;
            }

            IFormFile imagen = value as IFormFile;

            if (imagen.Length > pesoMaximo * 1024 * 1024) {

                return new ValidationResult($"El peso del archivo no debe ser mayor a { pesoMaximo } Mb");
            }

            return ValidationResult.Success;
        }
    }
}
