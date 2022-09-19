using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Validaciones
{
    public class TipoArchivoValid:ValidationAttribute
    {
        private readonly string[] grupoArchivo;

        public TipoArchivoValid(string[] grupoArchivo)
        {
            this.grupoArchivo = grupoArchivo;
        }

        public TipoArchivoValid(GrupoTipoArchivo grupoTipoArchivo)
        {
            if (grupoTipoArchivo == GrupoTipoArchivo.Imagen) {
                this.grupoArchivo = new string[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "application/pdf" };
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) {
                return ValidationResult.Success;
            }

            IFormFile archivo = value as IFormFile;

            if (archivo == null) {
                return ValidationResult.Success;
            }

            if (!this.grupoArchivo.Contains(archivo.ContentType)) {
                return new ValidationResult($"El tipo de archivo debe ser uno de los sgtes  {string.Join(",", this.grupoArchivo) } ");
            }

            return ValidationResult.Success;
        }
    }
}
