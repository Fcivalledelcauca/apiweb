using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace sinecoserveract.Servicios
{
    public class AlmacenadorArchivosAzure:IAlmacenadorArchivos
    {
        private readonly string connetionString;

        public AlmacenadorArchivosAzure(IConfiguration configuration)
        {
            this.connetionString = configuration.GetConnectionString("AzureStorage");
        }

        public async Task BorrarArchivo(string ruta, string carpeta)
        {
            if (string.IsNullOrEmpty(ruta))
            {
                return;
            }

            var cliente = new BlobContainerClient(connetionString, carpeta);
            await cliente.CreateIfNotExistsAsync();
            var archivo = Path.GetFileName(ruta);
            var blob = cliente.GetBlobClient(archivo);
            await blob.DeleteIfExistsAsync();

        }

        public async Task<string> EditarArchivo(byte[] contenido, string extension, string carpeta, string contentType, string ruta)
        {
            await BorrarArchivo(ruta, carpeta);
            return await GuardarArchivo(contenido, extension, carpeta, contentType);
        }

        public async Task<string> GuardarArchivo(byte[] contenido, string extension, string carpeta, string contentType)
        {
            var cliente = new BlobContainerClient(this.connetionString, carpeta);
            await cliente.CreateIfNotExistsAsync();
            cliente.SetAccessPolicy(PublicAccessType.Blob);

            var archivonombre = $"{Guid.NewGuid()}{extension}";
            var blob = cliente.GetBlobClient(archivonombre);

            var blobUploadOptions = new BlobUploadOptions();
            var blobHttpHeader = new BlobHttpHeaders();
            blobHttpHeader.ContentType = contentType;
            blobUploadOptions.HttpHeaders = blobHttpHeader;

            await blob.UploadAsync(new BinaryData(contenido), blobUploadOptions);
            return blob.Uri.ToString();

        }
    }
}
