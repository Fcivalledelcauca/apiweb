using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Servicios
{
    public interface IAlmacenadorArchivos
    {
        Task<string> GuardarArchivo(byte[] contenido, string extension, string carpeta, string contentType);
        Task<string> EditarArchivo(byte[] contenido, string extension, string carpeta, string contentType,string ruta);
        Task BorrarArchivo(string ruta, string carpeta);
    }
}
