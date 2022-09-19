using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Servicios
{
    public class AlmacenadorArchivosLocales : IAlmacenadorArchivos
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContexAccesor;

        public AlmacenadorArchivosLocales(IWebHostEnvironment env,IHttpContextAccessor httpContexAccesor)
        {
            this.env = env;
            this.httpContexAccesor = httpContexAccesor;
        }

        public Task BorrarArchivo(string ruta, string carpeta)
        {
            if (ruta != null) 
            {
                var nombreArchivo = Path.GetFileName(ruta);
                string directorioArchivo = Path.Combine(env.WebRootPath, carpeta, nombreArchivo);

                if (File.Exists(directorioArchivo)) {
                    File.Delete(directorioArchivo);
                }
            }

            return Task.FromResult(0);
        }

        public async Task<string> EditarArchivo(byte[] contenido, string extension, string carpeta, string contentType, string ruta)
        {
            await BorrarArchivo(ruta, carpeta);
            return await GuardarArchivo(contenido, extension, carpeta, contentType);
        }

        public async Task<string> GuardarArchivo(byte[] contenido, string extension, string carpeta, string contentType)
        {
            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(env.WebRootPath, carpeta);

            if (!Directory.Exists(folder)) 
            {
                Directory.CreateDirectory(folder);
            }

            string ruta = Path.Combine(folder, nombreArchivo);
            await File.WriteAllBytesAsync(ruta, contenido);

            var urlActual = $"{httpContexAccesor.HttpContext.Request.Scheme}://{httpContexAccesor.HttpContext.Request.Host}";
            var urlparaDB = Path.Combine(urlActual, carpeta, nombreArchivo).Replace("\\", "/");
            return urlparaDB;
        }
    }
}
