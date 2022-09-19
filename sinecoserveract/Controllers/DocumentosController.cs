using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sinecoserveract.Models;
using Microsoft.EntityFrameworkCore;
using sinecoserveract.Entidades;
using System.IO;
using sinecoserveract.Servicios;
using System.Text.RegularExpressions;
using sinecoserveract.DTOs;
using sinecoserveract.Utilidades;
using System.Text.Json;

namespace sinecoserveract.Controllers
{
    [ApiController]
    [Route("/api/documentos")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DocumentosController : ControllerBase
    {

        private readonly WebSIDataContext context;
        private readonly IConfiguration Configuration;
        private readonly IAlmacenadorArchivos almacenador;

        public DocumentosController(WebSIDataContext context, IConfiguration configuration, IAlmacenadorArchivos almacenador)
        {
            this.context = context;
            this.Configuration = configuration;
            this.almacenador = almacenador;
        }

        // GET: GruposController
        [HttpGet]
        public async Task<ActionResult> Index([FromQuery] PaginacionDTO paginacionDTO)
        {
            try
            {
                var queryable = (from docs in this.context.DfDocumentos
                                 join grupo in this.context.DfGrupos
                                 on docs.IdGrupo equals grupo.IdGrupo
                                 select new
                                 {
                                     idDocumento = docs.IdDocumento,
                                     idGrupo = docs.IdGrupo,
                                     nombreGrupo = grupo.Nombre_Grupo,
                                     idCia = docs.IdCia,
                                     titulo = docs.Titulo,
                                     descripcion = docs.Descripcion,
                                     codMunicipio = docs.CodMunicipio,
                                     nombreArchivo = docs.NombreArchivo,
                                     archivo = docs.Archivo,
                                     activo = docs.Activo

                                 }).AsQueryable();
                var cantidPaginas= await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDTO.CantidaddeRegistroPorPagina);
                var ctaDocs = await queryable.Paginar(paginacionDTO).ToListAsync();
                RespuestaPaginacion resP = new RespuestaPaginacion() { 
                    CantidadPaginas = cantidPaginas,
                    Respuesta =JsonSerializer.Serialize(ctaDocs)
                };
                

                return new JsonResult(resP);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro get: " + ex.Message);
            }
        }

        // GET: GruposController/Details/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var ctaDocs = await this.context.DfDocumentos.FirstOrDefaultAsync(c => c.IdDocumento == id);
                return new JsonResult(ctaDocs);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro get: " + ex.Message);
            }
        }

   
        // POST: GruposController/Create
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] DfDocumentos docs)
        {
            try
            {
                var yaExiste = await this.context.DfDocumentos.AnyAsync(d => d.CodMunicipio == docs.CodMunicipio && d.IdGrupo == docs.IdGrupo && d.Titulo == docs.Titulo);
                if (yaExiste) {
                    return BadRequest("El documento " + docs.Titulo + " ya existe por favor revisar");
                }


                var existe = await this.context.DfDocumentos.AnyAsync(g => g.IdDocumento == docs.IdDocumento && g.IdCia == docs.IdCia);
                if (existe)
                {
                    return BadRequest("El documento con titúlo " + docs.Titulo + " ya existe");
                }

                this.context.Add(docs);
                await this.context.SaveChangesAsync();
                return Ok(new JsonResult(docs));
            }
            catch (Exception ex)
            {
                return BadRequest("Error Post: " + ex.Message);
            }
        }

        // GET: GruposController/Edit/5
        [HttpPut]
        public async Task<ActionResult> Edit([FromBody] DfDocumentos docs)
        {
            try
            {
                var existe = await this.context.DfDocumentos.AnyAsync(g => g.IdDocumento== docs.IdDocumento);
                if (existe)
                {
                    this.context.Entry(docs).State = EntityState.Modified;
                    await this.context.SaveChangesAsync();
                    return Ok(new JsonResult(docs));
                }
                else
                {
                    return BadRequest("No es posible actualizar el documento");
                }
            }
            catch (Exception ex)
            {

                return BadRequest("Error Put: " + ex.Message);
            }
        }

        //delete
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var Existe = await this.context.DfDocumentos.AnyAsync(g => g.IdDocumento == id);
                if (Existe)
                {
                    var documento = await this.context.DfDocumentos.FindAsync(id);
                    if (documento != null)
                    {
                        this.context.DfDocumentos.Remove(documento);
                        await this.context.SaveChangesAsync();
                    }
                    return Ok(new JsonResult("El Documento fue eliminado Correctamente"));
                }
                else
                {
                    return BadRequest("Documento no existe " + id);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error Delete " + ex.Message);
            }
        }

        [HttpPost("uploadarchivo")]
        public async Task<ActionResult> UploadImagen([FromForm] ArchivosGrupoMunicipios archivos)
        {
            ResArchivo resArchivo = new ResArchivo();
            try
            {
                string rutaActual = "";

                if (archivos.Grupo == null || archivos.Municipio == null || archivos.titulo==null || archivos.Grupo == "" || archivos.Municipio == "" || archivos.titulo == "") { return BadRequest("No se puede procesar el archivo"); }
                string nombregrupo = Regex.Replace(archivos.Grupo, @"\s", "");
                string municipio = Regex.Replace(archivos.Municipio, @"\s", "");
                var docBD = await this.context.DfDocumentos.Where(d => d.CodMunicipio == municipio && d.IdGrupo == archivos.IdGrupo && d.Titulo == archivos.titulo).FirstOrDefaultAsync();
                if (docBD != null) {
                    rutaActual = docBD.Archivo;
                }
                else {
                    rutaActual = "";
                }

                //se pasa la informacion que el usuario modifica a la consulta hecha a la base de datos
               // string Carpeta = $"{municipio.ToLower()}{nombregrupo.ToLower()}";
                string Carpeta ="documentos";
                if (archivos.File != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await archivos.File.CopyToAsync(memoryStream);
                        var contenido = memoryStream.ToArray();
                        var nombreArchivo= Path.GetFileName(archivos.File.FileName);
                        var extension = Path.GetExtension(archivos.File.FileName);
                        string rutaArchivo = "";
                        if (string.IsNullOrEmpty(rutaActual))
                        {
                            rutaArchivo = await this.almacenador.GuardarArchivo(contenido, extension, Carpeta, archivos.File.ContentType);
                        }
                        else {
                            rutaArchivo = await this.almacenador.EditarArchivo(contenido, extension, Carpeta, archivos.File.ContentType, rutaActual);
                        }
                        resArchivo.NombreArchivo = nombreArchivo;
                        resArchivo.RutaArchivo = rutaArchivo;
                        
                    }
                }
            }
            catch (Exception ex)
            {

                return BadRequest("Error " + ex.Message);
            }
            return new JsonResult(resArchivo);
        }
    }
}
